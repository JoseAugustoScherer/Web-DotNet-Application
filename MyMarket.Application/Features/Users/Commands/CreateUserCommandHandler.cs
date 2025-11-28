using Microsoft.AspNetCore.Identity;
using MyMarket.Application.Abstractions;
using MyMarket.Application.Validators;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using CreateHandler = ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>>;

public class CreateUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, UserValidator validator) : CreateHandler
{
    public async Task<ResponseViewModel<Guid>> HandleAsync(CreateUserCommand command)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return ResponseViewModel<Guid>.Fail(errors, 400);
            }

            var user = new User(
                command.Name,
                command.LastName,
                command.Email,
                command.Amount,
                command.Password,
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
            return ResponseViewModel<Guid>.Fail(e.Message, 500);
        }
    }
}