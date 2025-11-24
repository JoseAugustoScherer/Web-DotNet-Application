using Microsoft.EntityFrameworkCore;
using MyMarket.Core.Entities;

namespace MyMarket.Infrastructure.Persistence;

public class MyMarketDbContext : DbContext
{
    public MyMarketDbContext(DbContextOptions<MyMarketDbContext> options) : base(options){}
    
    public DbSet<Product> Products { get; set; }
}