namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductDescriptionCommand(
    Guid Id,
    string Description);