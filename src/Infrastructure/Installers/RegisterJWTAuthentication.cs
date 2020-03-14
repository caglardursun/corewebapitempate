using API.Contracts;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace API.Infrastructure.Installers
{
    internal class RegisterJWTAuthentication: IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            #region  Identity server disabled 4 now
            //Setup JWT Authentication Handler with IdentityServer4
            //You should register the ApiName a.k.a Audience in your AuthServer
            //More info see: http://docs.identityserver.io/en/latest/topics/apis.html
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //        .AddIdentityServerAuthentication(options =>
            //        {
            //            options.Authority = config["ApiResourceBaseUrls:AuthServer"];
            //            options.RequireHttpsMetadata = false;
            //            options.ApiName = config["Self:Id"];
            //            options.ApiSecret = config["Self:Secret"];
            //            //options.CacheDuration = TimeSpan.FromDays(1);
            //        }); 
            #endregion


            #region JWT initialize ...
            //JWT 

            // configure jwt authentication

            var key = Encoding.ASCII.GetBytes(config["Self:Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        /*
                         validate issuer when you want to check/test the token was issued by an allowed server, 
                         if you don't validate the issuer anyone with the signing key can create a token that will 
                         be valid against your server 
                         */
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }); 
            #endregion
        }
    }
}
