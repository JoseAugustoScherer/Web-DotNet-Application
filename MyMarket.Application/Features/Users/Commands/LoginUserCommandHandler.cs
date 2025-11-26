using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyMarket.Application.Abstractions;
using MyMarket.Application.Features.Users.DTOs;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

public class LoginUserCommandHandler(IRepository<User> userRepository)
    : ICommandHandler<LoginUserCommand, ResponseViewModel<LoginUserResponse>>
{
    public async Task<ResponseViewModel<LoginUserResponse>> HandleAsync(LoginUserCommand command)
    {
        try
        {
            var user =  await userRepository.GetItemByAsync(x => x.Email == command.Email, default);

            if (user is null)
                return ResponseViewModel<LoginUserResponse>.Fail("User not found", StatusCodes.Status404NotFound);
        
            var hasher = new PasswordHasher<string>();

            if (hasher.VerifyHashedPassword(null, user.Password, command.Password) != PasswordVerificationResult.Success) 
                return ResponseViewModel<LoginUserResponse>.Fail("Password not found", StatusCodes.Status404NotFound);
        
            return ResponseViewModel<LoginUserResponse>.Ok(new LoginUserResponse(user.Id.ToString()));
        }
        catch (Exception e)
        {
            return ResponseViewModel<LoginUserResponse>.Fail(e.Message, StatusCodes.Status500InternalServerError);
        }
        
    }
}