namespace RentMotorcycle.Business.AppSettings
{
    public record RabbitMqOptions
    (
         string HostName,
         int Port,
         string UserName,
         string Password,
         string ExchangeName
    );
}
