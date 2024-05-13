using Microsoft.Extensions.Configuration;
using RentMotorcycleRabbitMQ.Interface;
using Microsoft.Extensions.DependencyInjection;
using RentMotorcycleRabbitMQ.Configuration;
using Microsoft.Extensions.Options;
using RentMotorcycle.Business.AppSettings;
using RentMotorcycleRabbitMQ.Models;

//public IConfiguration Configuration { get; }

//private static IServiceCollection ConfigureServices()
//{
//    IServiceCollection services = new ServiceCollection();
//    return services.ResolveDependencias();
//}

//public static async Task Main()
//{
//    var services = ConfigureServices();
//    var serviceProvider = services.BuildServiceProvider();
//    await serviceProvider.GetService<IStartProcess>().Init();
//}

var serviceCollection = new ServiceCollection();

// Configuração da leitura do appsettings.json
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Registra as configurações
var rabbitSettingsSection = configuration.GetSection("RabbitSettings");
var rabbitSettings = new RabbitSettings();
rabbitSettingsSection.Bind(rabbitSettings);

// Registra outros serviços necessários
// serviceCollection.AddSingleton<IService, ServiceImplementation>();

// Constrói o provedor de serviços
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolve as dependências necessárias
var appSettings = serviceProvider.GetRequiredService<IOptions<IRabbitSettings>>().Value;

// Agora você pode usar appSettings para acessar as configurações
Console.WriteLine($"ConnectionString: {appSettings.HostName}");

