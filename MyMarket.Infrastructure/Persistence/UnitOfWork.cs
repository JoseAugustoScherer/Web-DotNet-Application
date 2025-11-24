using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly MyMarketDbContext _dbContext;

    public UnitOfWork(MyMarketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<int> CommitAsync() => _dbContext.SaveChangesAsync();
}