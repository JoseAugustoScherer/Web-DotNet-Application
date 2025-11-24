namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductNameCommand(
    Guid Id,
    string Name
    );