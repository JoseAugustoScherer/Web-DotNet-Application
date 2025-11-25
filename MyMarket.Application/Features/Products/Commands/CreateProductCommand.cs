using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Products.Commands;

public sealed record CreateProductCommand
{
        public required string Name {get; init;}
        public required string Description {get; init;}
        public required Category Category {get; init;}
        public required decimal Price {get; init;}
        public required string Sku {get; init;}
        public required int Stock {get; init;}
        public DateTime CreatedOn {get; init;}
}