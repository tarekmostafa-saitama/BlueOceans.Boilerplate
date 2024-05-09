using System.Linq.Expressions;
using Application.Repositories.Specifications;

namespace Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> criteria,
        bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes);


    Task<IEnumerable<TEntity>> GetAllAsync(
        bool trackChanges = true,
        params Expression<Func<TEntity, object>>[] includes);


    Task<IEnumerable<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        params Expression<Func<TEntity, object>>[] includes);


    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);

    void RemoveRange(
        Expression<Func<TEntity, bool>> criteria);
}