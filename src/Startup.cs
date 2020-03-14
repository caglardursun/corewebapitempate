using API.Infrastructure.Configs;
using API.Infrastructure.Extensions;
using AspNetCoreRateLimit;
using AutoMapper;
using AutoWrapper;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Register services in Installers folder
            services.AddServicesInAssembly(Configuration);            

            //Register MVC/Web API, NewtonsoftJson and add FluentValidation Support
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options=> {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        })
                    .AddFluentValidation(fv => { fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; });            

            //Register Automapper
            services.AddAutoMapper(typeof(MappingProfileConfiguration));

            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "src/Static";
            //});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //Enable CORS
            app.UseCors("AllowAll");

            //Enable Swagger and SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ASP.NET Core API v1");
                c.DocumentTitle = "API Documentation";
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            //Enable HealthChecks and UI
            app.UseHealthChecks("/selfcheck", new HealthCheckOptions
            {
               Predicate = _ => true,
               ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }).UseHealthChecksUI();

            //Enable AutoWrapper.Core
            //More info see: https://github.com/proudmonkey/AutoWrapper
            app.UseApiResponseAndExceptionWrapper(
                new AutoWrapperOptions()
                {
                    ApiVersion = "v1",
                    IsApiOnly = false,
                    ShowApiVersion = true,
                    UseCamelCaseNamingStrategy = false,
                    ShowStatusCode = true,                    
                    IsDebug = true,
                    BypassHTMLValidation = true
                });

            //Enable AspNetCoreRateLimit
            app.UseIpRateLimiting();
            app.UseRouting();
            //Adds authenticaton middleware to the pipeline so authentication will be performed automatically on each request to host
            app.UseAuthentication();
            //Adds authorization middleware to the pipeline to make sure the Api endpoint cannot be accessed by anonymous clients
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();

            });




        }
    }
}
