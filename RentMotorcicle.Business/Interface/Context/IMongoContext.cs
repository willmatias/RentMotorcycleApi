using MongoDB.Driver;
using RentMotorcycle.Business.Interface.Repository;

namespace RentMotorcycle.Business.Interface.Context
{
    public  interface IMongoContext
    { 
        public IBaseMongoRepository<T> GetRepository<T>() where T : class;

        public IMongoCollection<T> GetCollection<T>();
    }
}
