namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductStockCommand(
    Guid Id,
    int Stock);