using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserEmailCommandHandlerTest
{
    private readonly UpdateUserEmailCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserEmailCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserEmailCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateUserEmailCommandHandlerTest_Success_WhenEmailIsValid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const string newEmail = "teste@gmail.com";
        var command = new UpdateUserEmailCommand(fakeUser.Id, newEmail);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        fakeUser.Email.Should().Be(newEmail);
    }
    
    [Fact]
    public async Task UpdateUserEmailCommandHandlerTest_Success_WhenEmailIsInvalid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const string newEmail = "teste@teste@gmail.com";
        var command = new UpdateUserEmailCommand(fakeUser.Id, newEmail);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);

        result.IsSuccess.Should().BeFalse();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        fakeUser.Email.Should().NotBe(newEmail);
    }
}