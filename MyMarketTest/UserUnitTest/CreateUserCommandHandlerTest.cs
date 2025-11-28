using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Application.Validators;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class CreateUserCommandHandlerTest
{
    private readonly CreateUserCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly UserValidator _validator;

    public CreateUserCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _validator = new UserValidator();
        _sut = new CreateUserCommandHandler(_repository.Object,  _unitOfWork.Object,  _validator);
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct_WhenProductDoesNotExist()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        
        var command = new CreateUserCommand(
            fakeUser.Name,
            fakeUser.LastName,
            fakeUser.Email,
            fakeUser.Amount,
            fakeUser.Password,
            fakeUser.Gender,
            fakeUser.BirthDate,
            fakeUser.Role,
            fakeUser.ActiveStatus);

        var userConsole = new UserConsoleOutPut();
        userConsole.ConsoleOutput(fakeUser);
        
        _repository.Setup(x => x.GetItemByAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null!);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.AddAsync(
                It.Is<User>(u => 
                    u.Name == fakeUser.Name &&
                    u.LastName == fakeUser.LastName &&
                    u.Email == fakeUser.Email &&
                    u.Amount == fakeUser.Amount &&
                    u.Password == fakeUser.Password &&
                    u.Gender == fakeUser.Gender &&
                    u.BirthDate == fakeUser.BirthDate &&
                    u.Role == fakeUser.Role &&
                    u.ActiveStatus == fakeUser.ActiveStatus
                ), It.IsAny<CancellationToken>()), 
            Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}