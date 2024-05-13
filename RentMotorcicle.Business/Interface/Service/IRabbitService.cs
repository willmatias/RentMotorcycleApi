using RentMotorcycle.Business.Models.Documents;

namespace RentMotorcycle.Business.Interface.Service
{
    public interface IRabbitService
    {
        void PublishMessageToRabbitQueue(Motors sendMessage, string queueName);
        void ReadQueue();
        void SendMessage(byte[] body, string queueName);
    }
}
