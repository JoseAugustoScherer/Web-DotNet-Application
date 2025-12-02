using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using MyMarket.Application.Abstractions;
using MyMarket.Application.Features.Users.DTOs;
using MyMarket.Application.Services;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, ResponseViewModel<LoginUserResponse>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IRepository<User> userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }
    
    public async Task<ResponseViewModel<LoginUserResponse>> HandleAsync(LoginUserCommand command)
    {
        try
        {
            Expression<Func<User, bool>> predicate = u => u.Email == command.Email;
            var user = await _userRepository.GetItemByAsync(predicate, CancellationToken.None);

            if (user is null)
                return ResponseViewModel<LoginUserResponse>.Fail("Invalid credentials.", StatusCodes.Status400BadRequest);
        
            var passwordHash = _authService.ComputeSha256Hash(command.Password);

            if (user.Password != passwordHash)
                return ResponseViewModel<LoginUserResponse>.Fail("Invalid credentials.", StatusCodes.Status400BadRequest);
        
            var token = _authService.GenerateJwtToken(user.Id, user.Email, user.Role.ToString());
            var loginResponse = new LoginUserResponse(token);

            return ResponseViewModel<LoginUserResponse>.Ok(loginResponse);
        }
        catch (Exception e)
        {
            var errorMessage = $"Error: {e.Message} --- StackTrace: {e.StackTrace}";
            Console.WriteLine(errorMessage);
            return ResponseViewModel<LoginUserResponse>.Fail(errorMessage, StatusCodes.Status500InternalServerError);
        }
    }
}