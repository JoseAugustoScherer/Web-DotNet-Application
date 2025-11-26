using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Infrastructure.Persistence;

public class UserRepository(MyMarketDbContext context) : Repository<User>(context), IUserRepository;