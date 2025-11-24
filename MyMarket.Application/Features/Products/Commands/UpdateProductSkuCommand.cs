namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductSkuCommand(
    Guid ProductId,
    string Sku);