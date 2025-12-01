using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserPasswordCommandHandlerTest
{
    private readonly UpdateUserPasswordCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserPasswordCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserPasswordCommandHandler(_repository.Object, _unitOfWork.Object);
    }
    
    [Fact]
    public async Task UpdateUserPasswordCommandHandlerTest_Success_WhenPasswordIsValid()
    {
        var userFake = FakeDataUser.FakeUserList(1).First();
        const string newPassword = "@Password123456";
        var command = new UpdateUserPasswordCommand(userFake.Id, newPassword);
        
        _repository.Setup(r => r.GetByIdAsync(userFake.Id, It.IsAny<CancellationToken>())).ReturnsAsync(userFake);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(userFake.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        userFake.Password.Should().Be(newPassword);
    }
    
    [Fact]
    public async Task UpdateUserPasswordCommandHandlerTest_Success_WhenPasswordIsInvalid()
    {
        var userFake = FakeDataUser.FakeUserList(1).First();
        const string newPassword = "Password123456";
        var command = new UpdateUserPasswordCommand(userFake.Id, newPassword);
        
        _repository.Setup(r => r.GetByIdAsync(userFake.Id, It.IsAny<CancellationToken>())).ReturnsAsync(userFake);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(userFake.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        userFake.Password.Should().NotBe(newPassword);
    }
}