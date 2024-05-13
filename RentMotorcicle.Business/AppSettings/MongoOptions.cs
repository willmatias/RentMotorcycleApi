namespace RentMotorcycle.Business.AppSettings
{
    public record MongoOptions
    {
        public string ConnectionString { get; init; }
        public string Database { get; init; }
        public bool IsSSL { get; init; }
    }
}
