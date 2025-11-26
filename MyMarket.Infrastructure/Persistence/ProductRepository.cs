using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Infrastructure.Persistence;

public class ProductRepository(MyMarketDbContext context) : Repository<Product>(context), IProductRepository;