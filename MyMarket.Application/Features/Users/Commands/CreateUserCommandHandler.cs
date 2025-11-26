using Microsoft.AspNetCore.Identity;
using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using CreateHandler = ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>>;

public class CreateUserCommandHandler : CreateHandler
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher<string> _hasher;

    public CreateUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _hasher = new PasswordHasher<string>();
        
    }
    
    public async Task<ResponseViewModel<Guid>> HandleAsync(CreateUserCommand command)
    {
        try
        {
            string password = _hasher.HashPassword(null, command.Password);
            
            var user = new User(
                command.Name,
                command.LastName,
                command.Email,
                command.Amount,
                password,
                command.Gender,
                command.BirthDate,
                command.Role,
                command.ActiveStatus);
            
            await _repository.AddAsync(user);
            await _unitOfWork.CommitAsync();
            
            return ResponseViewModel<Guid>.Ok(user.Id);
        }
        catch (Exception e)
        {
            return ResponseViewModel<Guid>.Fail(e.Message, 500);
        }
    }
}