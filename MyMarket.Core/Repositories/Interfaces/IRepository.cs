using System.Linq.Expressions;

namespace MyMarket.Core.Repositories.Interfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity> GetItemByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
}