using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace RentMotorcycle.Business.Interface.Repository
{
    public interface IBaseMongoRepository<T> where T: class
    {
        IBaseMongoRepository<T> GetRepositoryBase<T>() where T : class;

        void Insert(T entity);

        void Update(ObjectId id, T entity);

        void Delete(ObjectId id);

        T GetById(ObjectId id);
        List<T> GetAll();
        List<T> GetByField(string fieldName, object value);
        List<T> GetByFilter(Expression<Func<T, bool>> filter);

        Task InsertAsync(T entity);

        Task UpdateAsync(ObjectId id, T entity);
        Task<UpdateResult> UpdateFieldAsync<TField>(ObjectId id, Expression<Func<T, TField>> field, TField value);

        Task<DeleteResult> DeleteAsync(ObjectId id);

        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(ObjectId id);
        Task<List<T>> GetByFieldAsync(string fieldName, object value);
        Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetByBsonFilterAsync(FilterDefinition<T> filter);
    }
}
