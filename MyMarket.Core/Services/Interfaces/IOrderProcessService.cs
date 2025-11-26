using MyMarket.Core.Entities;

namespace MyMarket.Core.Services.Interfaces;

public interface IOrderProcessService
{
    public List<Product> AddProductToCart(Product product, int quantity);
    public bool ApprovedPurchaseOrder(User user, Product product);
}