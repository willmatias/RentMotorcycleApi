using RabbitMQ.Client;

namespace RentMotorcycle.Business.Interface.Context
{
    public interface IRabbitContext
    {
        IModel GetChannel();
    }
}
