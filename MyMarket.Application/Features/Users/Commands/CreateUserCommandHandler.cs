using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, ResponseViewModel<Guid>>
{
    private readonly IRepository<User> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IRepository<User> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseViewModel<Guid>> HandleAsync(CreateUserCommand command)
    {
        try
        {
            var user = new User(
                command.Name,
                command.LastName,
                command.Email,
                command.Password,
                command.Gender,
                command.BirthDate,
                command.Role,
                command.ActiveStatus,
                command.CreatedOn);
            
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