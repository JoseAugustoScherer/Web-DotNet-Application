namespace MyMarket.Application.Features.Products.Commands;

public record UpdateProductDescriptionCommand(
    Guid Id,
    string Description);