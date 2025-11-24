namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductPriceCommand(
    Guid Id,
    decimal Price);