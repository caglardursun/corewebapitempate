using API.Contracts;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using API.Infrastructure.Handlers;
using API.Data.DataManager;

namespace API.Infrastructure.Installers
{
    internal class RegisterDependencyMappings : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //register handler
            

            //Register Interface Mappings 4 Repositories Dep. Injection                                 
            //services.AddTransient<IGeneralUserManager, GeneralUserManager>();
                        

            //ApiAuthenticationService registered ...
            services.AddTransient<IApiAuthenticationService, ApiAuthenticationService>();
            services.AddTransient<IPasswordHashService, PasswordHashService>();

        }
    }
}
