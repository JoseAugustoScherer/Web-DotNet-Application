using MyMarket.Core.Entities;
using MyMarket.Core.Enums;

namespace MyMarketTest.ProductUnitTest;

public class CreateProductUnitTest
{
    [Theory]
    [InlineData("xUnitTestName", "xUnitTesteDescription", Category.Beauty, 12345, "Product001", 10)]
    [InlineData("Product", "Desc", Category.Automotive, 0.01, "SKU001", 0)]
    [InlineData("Product Especial", "Description with !@#$%¨&*()", Category.Furniture, 99999.99, "SKU-999", 1000)]
    public void CreateProductTest(
        string productName,
        string productDescription,
        Category productCategory,
        decimal productPrice,
        string productSku,
        int productStock)
    {
        var product = new Product(
            productName, 
            productDescription, 
            productCategory, 
            productPrice, 
            productSku, 
            productStock);
    
        Assert.NotNull(product);
        Assert.Equal(productName, product.Name);
        Assert.Equal(productDescription, product.Description);
        Assert.Equal(productCategory, product.Category);
        Assert.Equal(productPrice, product.Price);
        Assert.Equal(productSku, product.Sku);
        Assert.Equal(productStock, product.Stock);
    }
}