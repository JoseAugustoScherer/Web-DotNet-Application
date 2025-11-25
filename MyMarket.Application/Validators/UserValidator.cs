using FluentValidation;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Entities;

namespace MyMarket.Application.Validators;

public class UserValidator : AbstractValidator<CreateUserCommand>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be between 3 and 50 characters")
            .MaximumLength(50).WithMessage("Name must be between 3 and 50 characters");
        
        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MinimumLength(3).WithMessage("Last name must be between 3 and 50 characters")
            .MaximumLength(50).WithMessage("Last name must be between 3 and 50 characters");
            
    }
}