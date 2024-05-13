using RentMotorcycle.Business.Models.Security;

namespace RentMotorcycle.Business.AppSettings
{
    public sealed class AppSettings
    {    
        public RabbitMqOptions? RabbitConfiguration { get; set; }
        public MongoOptions? MongoConnection { get; set; }
        public Tokens? Tokens { get; set; }
    }
}
