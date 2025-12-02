using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using MyMarket.Application.Abstractions;
using MyMarket.Application.Services;
using MyMarket.Application.Validators;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using CreateHandler = ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>>;

public class CreateUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, UserValidator validator, IAuthService authService) : CreateHandler
{
    public async Task<ResponseViewModel<Guid>> HandleAsync(CreateUserCommand command)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return ResponseViewModel<Guid>.Fail(errors, StatusCodes.Status400BadRequest);
            }

            Expression<Func<User, bool>> predicate = u => u.Email == command.Email;
            var existingUser  = await repository.GetItemByAsync(predicate, CancellationToken.None);
            
            if (existingUser is not null)
                return ResponseViewModel<Guid>.Fail("This email address is already in use.", StatusCodes.Status400BadRequest);

            var passwordHash = authService.ComputeSha256Hash(command.Password);

            var user = new User(
                command.Name,
                command.LastName,
                command.Email,
                command.Amount,
                passwordHash,
                command.Gender,
                command.BirthDate,
                command.Role,
                command.ActiveStatus);
            
            await repository.AddAsync(user, CancellationToken.None); 
            await unitOfWork.CommitAsync(CancellationToken.None);
            
            return ResponseViewModel<Guid>.Ok(user.Id);
        }
        catch (Exception e)
        {
            return ResponseViewModel<Guid>.Fail("An internal server error has occurred.", StatusCodes.Status500InternalServerError);
        }
    }
}