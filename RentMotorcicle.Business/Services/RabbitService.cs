using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RentMotorcycle.Business.AppSettings;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Interface.Context;
using Microsoft.Extensions.Options;
using RentMotorcycle.Business.Models.Documents;
using RabbitMQ.Client.Events;

namespace RentMotorcycle.Business.Services
{
    public class RabbitService : IRabbitService
    {
        private readonly IRabbitContext _rabbitContext;
        private readonly RabbitMqOptions _rabbitOptions;
        public RabbitService(IRabbitContext rabbitContext,
            IOptions<RabbitMqOptions> rabbitOptions)
        {
            _rabbitContext = rabbitContext;
            _rabbitOptions = rabbitOptions.Value;
        }
        public void PublishMessageToRabbitQueue(Motors sendMessage, string queueName)
        {
            byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new List<Motors> { sendMessage }));
            SendMessage(body, queueName);
        }

        public void ReadQueue()
        {
            string queueName = "motors_save";
            _rabbitContext.GetChannel().QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _rabbitContext.GetChannel().ExchangeDeclare(exchange: _rabbitOptions.ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false);
            _rabbitContext.GetChannel().QueueBind(queue: queueName, exchange: _rabbitOptions.ExchangeName, routingKey: queueName);

            CreateEventingBasicConsumer(queueName);
        }

        public void SendMessage(byte[] body, string queueName)
        {
            _rabbitContext.GetChannel().QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _rabbitContext.GetChannel().ExchangeDeclare(exchange: _rabbitOptions.ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false);
            _rabbitContext.GetChannel().QueueBind(queue: queueName, exchange: _rabbitOptions.ExchangeName, routingKey: queueName);

            var basicProperties = _rabbitContext.GetChannel().CreateBasicProperties();
            basicProperties.Persistent = true;

            _rabbitContext.GetChannel().BasicPublish(exchange: _rabbitOptions.ExchangeName, routingKey: queueName, basicProperties: basicProperties, body: body);
        }

        private void CreateEventingBasicConsumer(string queueName)
        {

            var channel =_rabbitContext.GetChannel();
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());
                //var rabbitMessage = new RabbitMessage(ea.RoutingKey, message, false);

                //await _rabbitMessageService.AddRabbitMessage(rabbitMessage);

                Console.WriteLine("Lendo a fila: {0}", ea.RoutingKey);

                channel.BasicAck(ea.DeliveryTag, false);
            };            

            channel.BasicConsume(queue: queueName,
                                autoAck: false,
                                consumer: consumer);
        }
    }
}
