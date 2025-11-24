using FluentValidation;
using MyMarket.Application.Features.Products.Commands;

namespace MyMarket.Application.Validators;

public class ProductValidator : AbstractValidator<CreateProductCommand>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be between 3 and 50 characters")
            .MaximumLength(50).WithMessage("Name must be between 3 and 50 characters");

        RuleFor(product => product.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(255).WithMessage("The maximum length of Description is 255 characters");

        RuleFor(product => product.Sku)
            .NotEmpty().WithMessage("Sku is required");
        
        RuleFor(product => product.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
        
        RuleFor(product => product.Stock)
            .NotEmpty().WithMessage("Stock is required")
            .GreaterThan(0).WithMessage("Stock must be greater than 0");
    }
}