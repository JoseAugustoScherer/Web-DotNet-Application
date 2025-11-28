using MyMarket.Core.Entities;

namespace MyMarketTest.Utils.ProductTest;

// If you wanna see what kind of data the Bogus lib uses to create/update the fake product

public class ProductConsoleOutPut
{
    public void ConsoleOutput(Product product)
    {
        Console.WriteLine("\n\n=== FAKE DATA OF FAKE PRODUCT ===");
        Console.WriteLine($"ID: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Description: {product.Description}");
        Console.WriteLine($"Category: {product.Category}");
        Console.WriteLine($"Price: {product.Price:C}");
        Console.WriteLine($"SKU: {product.Sku}");
        Console.WriteLine($"Stock: {product.Stock}");
        Console.WriteLine("=================================\n\n");
    }
}