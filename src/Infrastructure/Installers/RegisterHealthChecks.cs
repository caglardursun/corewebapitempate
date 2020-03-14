using API.Contracts;
using API.Infrastructure.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace API.Infrastructure.Installers
{
    internal class RegisterHealthChecks : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //Register HealthChecks and UI
            services.AddHealthChecks()
                    .AddCheck("Çıkış Network Ping Durumu", new PingHealthCheck("www.google.com", 100))                    
                    .AddNpgSql(
                                config["ConnectionStrings:PostgreSQLConnectionString"],                                
                                "PosgreSQL",
                                null,
                                "PosgreSQL",
                                HealthStatus.Degraded,
                                new string[] { "db", "sql", "sqlserver" }
                    );

            services.AddHealthChecksUI();
        }
    }
}
