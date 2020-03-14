using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using API.Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.Filters
{
    public class FormFileOperationFilter : IOperationFilter
    {
        
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var parameters = operation.Parameters;
            if (parameters == null || parameters.Count == 0)
                return;


            var isFormFileFound = false;


            if (isFormFileFound)
            {                

                operation.RequestBody = new OpenApiRequestBody()
                {
                    Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Type = "object",
                            Properties =
                            {
                                ["file"] = new OpenApiSchema()
                                {
                                    Description = "Select file", Type = "string", Format = "binary"
                                }
                            }
                        }
                    }
                }
                };
            }
        }
    }
}
