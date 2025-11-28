using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Infrastructure.Persistence;

public class Repository<TEntity>(MyMarketDbContext dbContext) : IRepository<TEntity>, IAsyncDisposable where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Add(entity);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Remove(entity);
    }

    public async Task<TEntity?> GetItemByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public void Dispose()
    {
        dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }

}