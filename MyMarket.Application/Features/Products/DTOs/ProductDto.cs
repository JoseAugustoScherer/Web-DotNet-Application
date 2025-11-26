using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    Category ProductCategory,
    decimal Price,
    string Sku,
    int Stock);