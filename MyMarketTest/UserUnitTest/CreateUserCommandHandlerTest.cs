using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarketTest.UserUnitTest;

public class CreateUserCommandHandlerTest
{
    private readonly LoginUserCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;

    public CreateUserCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        
        _sut = new LoginUserCommandHandler(_repository.Object);
    }

    [Fact]
    public async Task FailEmail()
    {
        var command = new LoginUserCommand("teste@email.com", "123456");
        
        var hasher = new PasswordHasher<string>();
        
        var password = hasher.HashPassword(null, "123");
        
        var user = new User("name", "lastName", "lastName@email.com", 0, password, Gender.Other, DateTime.Now, Role.Administrator, ActiveStatus.Active);

        _repository
            .Setup(x => x.GetItemByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            // .ReturnsAsync((User)null);
            .ReturnsAsync(user);
            // .Throws(new Exception());
        
        var result = await _sut.HandleAsync(command);
        
        Assert.True(result.IsFailure);
    }
}