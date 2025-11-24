using MyMarket.Core.Entities;
using MyMarket.Core.Enums;

namespace MyMarket.Core.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>?> GetByCategory(Category category);
    Task<Product?> GetBySku(string sku);
}