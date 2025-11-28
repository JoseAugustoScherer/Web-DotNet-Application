using Bogus;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;

namespace MyMarketTest.Utils.ProductTest;

public static class FakeDataProducts
{
    public static List<Product> FakeProductList(int count)
    {
        var productFaker = new Faker<Product>("en")
            .RuleFor(p => p.Id, 
                f => f.Random.Guid())
            .RuleFor(p => p.Name, 
                f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, 
                f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Category, 
                f => f.PickRandom<Category>())
            .RuleFor(p => p.Price, 
                f => f.Random.Decimal(1, 2000))
            .RuleFor(p => p.Sku, 
                f => $"SKU{f.Random.Number(10000, 99999)}")
            .RuleFor(p => p.Stock, 
                f => f.Random.Int(1, 2000));

        var products = productFaker.Generate(count);

        return products;
    }
}