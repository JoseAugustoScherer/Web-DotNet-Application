using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.Commands;

public sealed record UpdateProductCategoryCommand(
    Guid Id,
    Category Category);