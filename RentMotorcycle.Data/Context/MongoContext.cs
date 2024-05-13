using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RentMotorcycle.Business.AppSettings;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Repository;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Utils;
using RentMotorcycle.Data.Repository;
using System.Collections.Concurrent;

namespace RentMotorcycle.Data.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly ConcurrentDictionary<Type, object> _repositoryInstances = new ConcurrentDictionary<Type, object>();



        public MongoContext(IOptions<MongoOptions> omniOptions)
        {
            var omniOptionsValues = omniOptions.Value;

            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(omniOptionsValues.ConnectionString));

                if (omniOptionsValues.IsSSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }

                settings.ConnectTimeout = TimeSpan.FromSeconds(10);

                MongoClient mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(omniOptionsValues.Database);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }

        }

        public IBaseMongoRepository<T> GetRepository<T>() where T : class
        {
            return (IBaseMongoRepository<T>)_repositoryInstances.GetOrAdd(typeof(T), _ =>
            {
                var repository = new BaseMongoRepository<T>(this);
                return repository.GetRepositoryBase<T>();
            });
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            string collectionName = GetCollectionName<T>();
            return _database.GetCollection<T>(collectionName);
        }

        private static string GetCollectionName<T>()
        {
            var attribute = typeof(T).GetCustomAttributes(typeof(CollectionNameAttribute), true)
                                     .FirstOrDefault() as CollectionNameAttribute;

            return attribute?.Name ?? typeof(T).Name.ToLowerInvariant();
        }
    }
}
