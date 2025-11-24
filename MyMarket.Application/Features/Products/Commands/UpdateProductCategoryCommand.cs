using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.Commands;

public record UpdateProductCategoryCommand(
    Guid Id,
    Category Category);