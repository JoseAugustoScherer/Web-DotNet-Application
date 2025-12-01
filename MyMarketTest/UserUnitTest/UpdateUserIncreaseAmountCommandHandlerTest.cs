using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserIncreaseAmountCommandHandlerTest
{
    private readonly UpdateUserIncreaseAmountCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserIncreaseAmountCommandHandlerTest()
    {
        _repository = new  Mock<IUserRepository>();
        _unitOfWork = new  Mock<IUnitOfWork>();
        _sut = new UpdateUserIncreaseAmountCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateUserIncreaseAmountCommandHandlerTest_Success_WhenAmountIsValid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var originalAmount = fakeUser.Amount;
        const decimal newAmount = 200;
        var command = new UpdateUserIncreaseAmountCommand(fakeUser.Id, newAmount);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        result.IsSuccess.Should().BeTrue();
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        fakeUser.Amount.Should().Be(originalAmount + newAmount);
    }
    
    [Fact]
    public async Task UpdateUserIncreaseAmountCommandHandlerTest_Success_WhenAmountIsInvalid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var originalAmount = fakeUser.Amount;
        const decimal newAmount = -100;
        var command = new UpdateUserIncreaseAmountCommand(fakeUser.Id, newAmount);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        result.IsFailure.Should().BeTrue();
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        fakeUser.Amount.Should().NotBe(originalAmount + newAmount);
    }
}