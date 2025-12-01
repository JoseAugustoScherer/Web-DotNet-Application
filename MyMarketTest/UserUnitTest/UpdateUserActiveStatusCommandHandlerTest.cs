using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserActiveStatusCommandHandlerTest
{
    private readonly UpdateUserActiveStatusCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserActiveStatusCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserActiveStatusCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task UpdateUserBirthDateCommandHandlerTest_Success_WhenStatusCodeIsValid(int validStatusCode)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var status = (ActiveStatus)validStatusCode;
        var command = new UpdateUserActiveStatusCommand(fakeUser.Id, status);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        fakeUser.ActiveStatus.Should().Be(status);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public async Task UpdateUserBirthDateCommandHandlerTest_Failure_WhenStatusCodeIsInvalid(int validStatusCode)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var status = (ActiveStatus)validStatusCode;
        var command = new UpdateUserActiveStatusCommand(fakeUser.Id, status);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Never);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        fakeUser.ActiveStatus.Should().NotBe(status);
    }
}