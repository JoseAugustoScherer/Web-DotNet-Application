namespace MyMarket.Core.Repositories.Interfaces;

public interface IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken);
}