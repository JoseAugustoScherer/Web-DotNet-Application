using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyMarket.Infrastructure.Persistence;

public class MyMarketDbContextFactory : IDesignTimeDbContextFactory<MyMarketDbContext>
{
    public MyMarketDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyMarketDbContext>();

        optionsBuilder.UseSqlite("Data Source=MyMarket.db");

        return new MyMarketDbContext(optionsBuilder.Options);
    }
}