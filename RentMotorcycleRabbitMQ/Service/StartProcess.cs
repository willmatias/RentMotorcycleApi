using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentMotorcycleRabbitMQ.Interface;
using RentMotorcycleRabbitMQ.Models;
using System.Text;


namespace RentMotorcycleRabbitMQ.Service
{
    public class StartProcess : IStartProcess
    {

        private readonly IRabbitSettings _rabbitSettings;
        public StartProcess(IRabbitSettings rabbitSettings)
        {
            _rabbitSettings = rabbitSettings;
        }
        public async Task Init()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = _rabbitSettings.HostName,
                Port = _rabbitSettings.Port,
                UserName = _rabbitSettings.UserName,
                Password = _rabbitSettings.Password
            };

            IConnection connection = factory.CreateConnection();

            IModel channel;
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName, type: ExchangeType.Topic, true);

            CreateEventingBasicConsumer(channel);

        }

        private void CreateEventingBasicConsumer(IModel channel)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                //var rabbitMessage = new RabbitMessage(ea.RoutingKey, message, false);

                //await _rabbitMessageService.AddRabbitMessage(rabbitMessage);

                Console.WriteLine("lendo a fila do rabbitmq: {0}", ea.RoutingKey);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            var queueName = channel.QueueDeclare("motors_service", true, false, false).QueueName;

            channel.QueueBind(queue: queueName,
                                    exchange: _rabbitSettings.ExchangeName,
                                    routingKey: queueName);

            channel.BasicConsume(queue: queueName,
                                autoAck: false,
                                consumer: consumer);
        }

    }

}
