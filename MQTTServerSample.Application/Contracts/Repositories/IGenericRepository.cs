using System.Linq.Expressions;

namespace MQTTServerSample.Application.Contracts.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class, IBaseTable
{
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> GetAllRaw(Expression<Func<TEntity, bool>>? predicate);
    IEnumerable<TEntity> GetAllDeleted(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetById(Guid id);
    Task<TEntity> Create(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task<bool> Delete(Guid id);
    Task<bool> SoftDelete(Guid id, string cUserId);
    Task<bool> Save();
    Task<bool> ExecuteQuery(string query);
    Task<bool> Exist(Guid id);
}