using PenMail.Constants;
using PenMail.Contracts;
using PenMail.Infrastructure.Configs;
using PenMail.Infrastructure.Handlers;
using PenMail.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PenMail.Infrastructure.Installers
{
    internal class RegisterApiResources : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            

            var policyConfigs = new HttpClientPolicyConfiguration();
            config.Bind("HttpClientPolicies", policyConfigs);

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(policyConfigs.RetryTimeoutInSeconds));

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(policyConfigs.RetryCount, _ => TimeSpan.FromMilliseconds(policyConfigs.RetryDelayInMs));

            var circuitBreakerPolicy = HttpPolicyExtensions
               .HandleTransientHttpError()
               .CircuitBreakerAsync(policyConfigs.MaxAttemptBeforeBreak, TimeSpan.FromSeconds(policyConfigs.BreakDurationInSeconds));
            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            

            #region Disabled

            //Register custom Bearer Token Handler. The DelegatingHandler has to be registered as a Transient Service
            //services.AddTransient<ProtectedApiBearerTokenHandler>();

            //Register a Typed Instance of HttpClientFactory for a Protected Resource
            //More info see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0


            //services.AddHttpClient<IApiConnect, CustomerApiConnect>(client =>
            //{
            //    client.BaseAddress = new Uri(config["ApiResourceBaseUrls:CustomerApi"]);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentMediaTypes.JSON));
            //})
            //.SetHandlerLifetime(TimeSpan.FromMinutes(policyConfigs.HandlerTimeoutInMinutes))
            //.AddHttpMessageHandler<ProtectedApiBearerTokenHandler>()
            //.AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOpPolicy)
            //.AddPolicyHandler(timeoutPolicy)
            //.AddPolicyHandler(circuitBreakerPolicy);



            //Register a Typed Instance of HttpClientFactory for AuthService 
            //services.AddHttpClient<IAuthenticationManager, AuthServerConnect>();

            // Register the DiscoveryCache in DI and will use the HttpClientFactory to create clients. Cached for 24hrs by default
            //services.AddSingleton<IDiscoveryCache>(r =>
            //{
            //    var factory = r.GetRequiredService<IHttpClientFactory>();
            //    return new DiscoveryCache(config["ApiResourceBaseUrls:AuthServer"], () => factory.CreateClient());
            //});

            #endregion

           
        }
    }
}


