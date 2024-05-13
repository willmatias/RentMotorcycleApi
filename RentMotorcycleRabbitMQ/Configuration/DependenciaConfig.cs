using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentMotorcycle.Business.AppSettings;
using RentMotorcycleRabbitMQ.Interface;
using RentMotorcycleRabbitMQ.Models;
using RentMotorcycleRabbitMQ.Service;

namespace RentMotorcycleRabbitMQ.Configuration
{
    public static class DependenciaConfig
    {
        public static IServiceCollection ResolveDependencias(this IServiceCollection services)
        {
            services.AddScoped<IStartProcess, StartProcess>();

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            Console.WriteLine("passou aqui");
            Console.WriteLine(configuration.GetSection("RabbitSettings").Get<RabbitSettings>());            
           
            services.AddSingleton<IRabbitSettings>(configuration.GetSection("RabbitSettings").Get<RabbitSettings>());

            

            return services;
        }
    }
}
