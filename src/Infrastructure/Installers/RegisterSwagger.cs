using API.Contracts;
using API.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;

namespace API.Infrastructure.Installers
{
    internal class RegisterSwagger: IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //Register Swagger
            //See: https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "ASP.NET Core API"
                    ,Version = "v1" 
                    ,Description = "API Web Servisler Dökümantasyonu"
                    ,Contact = new OpenApiContact
                    {
                        Name = "Çağlar Dursun",
                        Email = "dursuncaglar[~at~]~.com"                        
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                //... and tell Swagger to use those XML comments.
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",                    
                    BearerFormat = "jtw",
                    In = ParameterLocation.Header,                    
                    Description = "JWT Authorization header using the Bearer scheme."
                    
                });
                

                //Bu aq. Bearer keyword ü otomatik olarak eklemediği için 
                //elle ekleme yapacaksın value kısmına 
                //Bknz : https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/603
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                //options.OperationFilter<SwaggerAuthorizeCheckOperationFilter>();
                //options.OperationFilter<FormFileOperationFilter>();
                
            });

        
           
        }
    }
}
