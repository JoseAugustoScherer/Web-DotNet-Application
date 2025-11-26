using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.Commands;

public sealed record CreateProductCommand(
    string Name, 
    string Description, 
    Category ProductCategory, 
    decimal Price,
    string Sku,
    int Stock);