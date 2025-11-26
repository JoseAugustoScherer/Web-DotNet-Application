using MyMarket.Core.Entities;
using MyMarket.Core.Services.Interfaces;

namespace MyMarket.Core.Services.Implementations;

public class OrderProcess : IOrderProcessService
{
    public List<Product> AddProductToCart(Product product, int quantity)
    {
        throw new NotImplementedException();
    }

    public bool ApprovedPurchaseOrder(User user, Product product)
    {
        throw new NotImplementedException();
    }
}