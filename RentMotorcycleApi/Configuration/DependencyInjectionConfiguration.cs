using Microsoft.Extensions.Options;
using RentMotorcycle.Business.AppSettings;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Security;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Security;
using RentMotorcycle.Business.Services;
using RentMotorcycle.Data.Context;

namespace RentMotorcycleApi.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureLifeTime(this IServiceCollection serviceCollection, AppSettings appSettings)
        {

            serviceCollection.AddSingleton(Options.Create(appSettings.MongoConnection));
            serviceCollection.AddSingleton(Options.Create(appSettings.RabbitConfiguration));
            serviceCollection.AddSingleton(Options.Create(appSettings.Tokens));
            serviceCollection.AddSingleton<IRabbitContext, RabbitContext>();
            serviceCollection.AddSingleton<IRabbitService, RabbitService>();
            serviceCollection.AddSingleton<IMotorsService, MotorsService>();

            serviceCollection.AddSingleton<IMongoContext, MongoContext>();
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<IIdentity, Identity>();


        }
    }
}
