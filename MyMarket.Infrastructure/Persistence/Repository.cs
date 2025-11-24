using Microsoft.EntityFrameworkCore;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Infrastructure.Persistence;

public class Repository : IProductRepository
{
    private readonly MyMarketDbContext _dbContext;
    
    public Repository(MyMarketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Set<Product>().ToListAsync();
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new Exception("Id cannot be empty");
        
        var entity = await _dbContext.Set<Product>().FindAsync(id);
        
        if (entity == null)
            throw new Exception("No entity found");
        
        return entity;
    }

    public Task UpdateAsync(Product entity)
    {
        _dbContext.Set<Product>().Update(entity);
        return Task.CompletedTask;
    }

    public Task AddAsync(Product entity)
    {
        _dbContext.Set<Product>().Add(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product entity)
    {
        _dbContext.Set<Product>().Remove(entity);
        return Task.CompletedTask;
    }
    
    public async Task<IEnumerable<Product>?> GetByCategory(Category category)
    {
        var results = await _dbContext.Set<Product>().Where(product => product.Category == category).ToListAsync();
        
        if (results == null)
            throw new Exception($"No entity with ${category} found");
        
        return results; 
    }

    public async Task<Product?> GetBySku(string sku)
    {
        var result = await _dbContext.Set<Product>().FindAsync(sku);
        
        if (result == null)
            throw new Exception($"No entity with ${sku} found");
        
        return result; 
    }
}