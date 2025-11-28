using System.Data;
using System.Text.RegularExpressions;
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
        
        const string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        
        RuleFor(user => user.Email)
              .NotEmpty().WithMessage("Email is required")
              .EmailAddress().WithMessage("Email is invalid")
              .MaximumLength(255).WithMessage("Email is to long. The maximum length is 255 characters.")
              .Matches(emailRegex, RegexOptions.IgnoreCase).WithMessage("The email format is invalid.");
              
        RuleFor(x => x.Password)
              .NotEmpty().WithMessage("The password is required")
              .MinimumLength(8).WithMessage("The password is to short. The minimum length is 8")
              .Matches(@"[A-Z]").WithMessage("The password must contain at least one stored letter.")
              .Matches(@"[a-z]").WithMessage("The password must contain at least one lowercase letter.")
              .Matches(@"\d").WithMessage("The password must contain at least one number.")
              .Matches(@"[#@$!%*?&]").WithMessage("The password must contain at least one special character.");
              
          // RuleFor(user => user.ConfirmPassword)
          //     .Equal(user => user.Password).WithMessage("Passwords do not match");
          
          RuleFor(user => user.Gender)
              .IsInEnum().WithMessage("Gender is invalid");
          
          RuleFor(user => user.BirthDate)
              .GreaterThan(DateTime.MinValue).WithMessage("BirthDate is required");
          
          RuleFor(user => user.Role)
              .IsInEnum().WithMessage("Gender is invalid");
            
          RuleFor(user => user.ActiveStatus)
              .IsInEnum().WithMessage("Active status is invalid");
    }
}