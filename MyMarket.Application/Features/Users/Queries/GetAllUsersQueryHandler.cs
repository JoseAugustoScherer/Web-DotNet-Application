using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, ResponseViewModel>
{
    private readonly IRepository<User> _repository;

    public GetAllUsersQueryHandler(IRepository<User> userRepository)
    {
        _repository = userRepository;
    }
    
    public async Task<ResponseViewModel> HandleAsync(GetAllUsersQuery query)
    {
        try
        {
            var users = await _repository.GetAllAsync();

            var userDto = users.Select(u => new UserDto(
                u.Id,
                u.Name,
                u.LastName,
                u.Email,
                u.Password,
                u.Gender,
                u.BirthDate,
                u.Role,
                u.ActiveStatus,
                u.CreatedOn)).ToList();

            return ResponseViewModel<List<UserDto>>.Ok(userDto);
        }
        catch (Exception e)
        {
            return  ResponseViewModel<List<UserDto>>.Fail(e.Message, 500);
        }
    }
}