using Microsoft.EntityFrameworkCore;
using MyMarket.Core.Entities;

namespace MyMarket.Infrastructure.Persistence;

public class MyMarketDbContext : DbContext
{
    public MyMarketDbContext(DbContextOptions<MyMarketDbContext> options) : base(options){}
    
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));
        
        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).ModifiedOn = DateTime.UtcNow;
        
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedOn = DateTime.UtcNow;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}