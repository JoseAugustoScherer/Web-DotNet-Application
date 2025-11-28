using System.Linq.Expressions;

namespace MyMarket.Core.Repositories.Interfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> GetItemByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
}