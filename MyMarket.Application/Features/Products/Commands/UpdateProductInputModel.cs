using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductInputModel(
    string Name,
    string Description,
    Category Category,
    decimal Price, 
    string Sku, 
    int Stock);