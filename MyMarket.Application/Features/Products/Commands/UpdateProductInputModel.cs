using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.Commands;

public record UpdateProductInputModel(
    string Name,
    string Description,
    Category Category,
    decimal Price, 
    string Sku, 
    int Stock);