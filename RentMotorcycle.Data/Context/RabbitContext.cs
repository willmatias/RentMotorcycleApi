using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RentMotorcycle.Business.AppSettings;
using RentMotorcycle.Business.Interface.Context;

namespace RentMotorcycle.Data.Context
{
    public class RabbitContext : IRabbitContext
    {
        private readonly object _lock = new object();
        private readonly IOptions<RabbitMqOptions> _options;
        private readonly ILogger<RabbitContext> _logger;

        private IConnection _connection;
        private IModel _channel;

        private IConnection Connection
        {
            get
            {
                EnsureConnection();
                return _connection;
            }
        }

        public IModel GetChannel()
        {
            EnsureConnection();

            lock (_lock)
            {
                if (_channel == null || !_channel.IsOpen)
                {
                    _channel = _connection.CreateModel();
                }

                return _channel;
            }
        }

        public RabbitContext(
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitContext> logger)
        {
            _logger = logger;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            InitializeConnection();
            InitializeHealthCheck();
        }

        private void InitializeConnection()
        {
            var rabbitOptions = _options.Value;

            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                Port = rabbitOptions.Port,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password
            };

            const int maxRetryAttempts = 10;
            int currentAttempt = 1;

            while (true)
            {
                try
                {
                    _connection = connectionFactory.CreateConnection();
                    _logger.LogInformation("Conectado ao Rabbit com Sucesso.");

                    break;
                }
                catch (Exception ex)
                {
                    if (currentAttempt > maxRetryAttempts)
                    {
                        _logger.LogError($@"Falha ao conectar com Rabbit. Excecao : {ex}");
                        break;
                    }

                    currentAttempt++;

                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            }
        }

        private void EnsureConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                lock (_lock)
                {
                    if (_connection == null || !_connection.IsOpen)
                    {
                        InitializeConnection();
                    }
                }
            }
        }

        private void InitializeHealthCheck()
        {
            var healthCheckTimer = new Timer(HealthCheck, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            void HealthCheck(object state)
            {
                Console.WriteLine("Health Check");
                if (!Connection.IsOpen)
                {
                    _logger.LogInformation($"Health Check - Conexão não estava aberta. Tentando reinicializar a conexão!");
                    InitializeConnection();
                }
            }
        }
    }
}
