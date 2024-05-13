namespace RentMotorcycleRabbitMQ.Models
{
    public interface IRabbitSettings
    {
        string HostName { get; set; }
        string Password { get; set; }        
        string UserName { get; set; }
        int Port { get; set; }
        string ExchangeName { get; set; }
    }

    public class RabbitSettings : IRabbitSettings
    {
        public string HostName { get; set; }
        public string Password { get; set; }        
        public string UserName { get; set; }
        public int Port { get; set; }
        public string ExchangeName { get; set; }
    }
}
