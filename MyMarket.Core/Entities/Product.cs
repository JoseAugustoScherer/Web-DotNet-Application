using MyMarket.Core.Enums;

namespace MyMarket.Core.Entities;

public sealed class Product(
    string name,
    string description,
    Category category,
    decimal price,
    string sku,
    int stock) : BaseEntity
{
    
    public Guid Id { get; private set; } =  Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string Description { get; private set; }  = description;
    public Category Category { get; private set; } = category;
    public decimal Price { get; private set; } = price;
    public string Sku { get; private set; }  = sku;
    public int Stock { get; private set; } =  stock;

    public void UpdateName(
        string name)
    {
        ValidFields(name);
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        ValidFields(description);
        Description = description;
    }
    
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price cannot be negative.");
        
        Price = newPrice;
    }

    public void UpdateSku(string sku)
    {
        ValidFields(sku);
        Sku = sku;
    }

    public void UpdateCategory(Category categoryId)
    {
        Category =  categoryId;
    }
    
    public void UpdateStock(int amount)
    {
        ValidQuantity(amount);
        
        Stock = amount;
    }
    
    public void IncreaseStock(int amount)
    {
        ValidQuantity(amount);
        
        Stock += amount;
    }

    public void DecreaseStock(int amount)
    {
        ValidQuantity(amount);
        
        if (Stock - amount < 0)
            throw new InvalidOperationException("Stock cannot be negative.");
        
        Stock -= amount;
    }
    
    private static void ValidQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");
    }

    private static void ValidFields(string field)
    {
        if (string.IsNullOrWhiteSpace(field))
            throw new ArgumentException($"Required field '{field}' is missing.");
    }
}