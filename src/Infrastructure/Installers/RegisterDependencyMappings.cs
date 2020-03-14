using PenMail.Contracts;
using PenMail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenMail.Infrastructure.Handlers;
using PenMail.Data.DataManager;

namespace PenMail.Infrastructure.Installers
{
    internal class RegisterDependencyMappings : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //register handler
            services.AddTransient<IFileWriterHandler, FileWriterHandler>();

            //Register Interface Mappings 4 Repositories Dep. Injection                                 
            //services.AddTransient<IPersonManager, PersonManager>();
                        

            //ApiAuthenticationService registered ...
            services.AddTransient<IApiAuthenticationService, ApiAuthenticationService>();
            services.AddTransient<IPasswordHashService, PasswordHashService>();

        }
    }
}
