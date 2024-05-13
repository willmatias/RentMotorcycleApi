using MongoDB.Bson;
using MongoDB.Driver;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Repository;
using System.Linq.Expressions;

namespace RentMotorcycle.Data.Repository
{
    public class BaseMongoRepository<T> : IBaseMongoRepository<T> where T : class
    {
        public readonly IMongoContext _mongoContext;

        public BaseMongoRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public IBaseMongoRepository<T> GetRepositoryBase<T>() where T : class
        {
            return (IBaseMongoRepository<T>)this;
        }

        public T GetById(ObjectId id)
        {
            return _mongoContext.GetCollection<T>()
                .Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefault();
        }

        public List<T> GetAll()
        {
            return _mongoContext.GetCollection<T>()
                .Find(new BsonDocument()).ToList();
        }

        public void Insert(T entity)
        {
            _mongoContext.GetCollection<T>()
                .InsertOne(entity);
        }

        public void Update(ObjectId id, T entity)
        {
            _mongoContext.GetCollection<T>()
                .ReplaceOne(Builders<T>.Filter.Eq("_id", id), entity);
        }

        public void Delete(ObjectId id)
        {
            _mongoContext.GetCollection<T>()
                .DeleteOne(Builders<T>.Filter.Eq("_id", id));
        }

        public List<T> GetByField(string fieldName, object value)
        {
            var filter = Builders<T>.Filter.Eq(fieldName, value);
            return _mongoContext.GetCollection<T>()
                .Find(filter).ToList();
        }

        public List<T> GetByFilter(Expression<Func<T, bool>> filter)
        {
            return _mongoContext.GetCollection<T>()
                .Find(filter).ToList();
        }

        public async Task<T> GetByIdAsync(ObjectId id)
        {
            return await _mongoContext.GetCollection<T>()
                .Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _mongoContext.GetCollection<T>()
                .Find(new BsonDocument()).ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _mongoContext.GetCollection<T>()
                .InsertOneAsync(entity);
        }

        public async Task UpdateAsync(ObjectId id, T entity)
        {
            await _mongoContext.GetCollection<T>()
                .ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
        }

        public async Task UpdateFieldAsync<TField>(ObjectId id, Expression<Func<T, TField>> field, TField value)
        {
            var update = Builders<T>.Update.Set(field, value);
            await _mongoContext.GetCollection<T>()
                .UpdateOneAsync(Builders<T>.Filter.Eq("_id", id), update);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _mongoContext.GetCollection<T>()
                .DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }

        public async Task<List<T>> GetByFieldAsync(string fieldName, object value)
        {
            var filter = Builders<T>.Filter.Eq(fieldName, value);
            return await _mongoContext.GetCollection<T>()
                .Find(filter).ToListAsync();
        }

        public async Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await _mongoContext.GetCollection<T>()
                .Find(filter).ToListAsync();
        }

        public async Task<List<T>> GetByBsonFilterAsync(FilterDefinition<T> filter)
        {
            return await _mongoContext.GetCollection<T>()
                .Find(filter).ToListAsync();

        }
    }
}
