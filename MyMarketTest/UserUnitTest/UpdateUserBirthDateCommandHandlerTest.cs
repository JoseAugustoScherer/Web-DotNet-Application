using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserBirthDateCommandHandlerTest
{
    private readonly UpdateUserBirthDateCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserBirthDateCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserBirthDateCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Theory]
    [InlineData(2001, 11, 24)]
    [InlineData(2000, 6, 28)]
    [InlineData(2004, 7, 13)]
    public async Task UpdateUserBirthDateCommandHandlerTest_Success_WhenBirthDateValid(int year, int month, int day)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var birthDate = new DateTime(year, month, day);
        var command = new UpdateUserBirthDateCommand(fakeUser.Id, birthDate);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        result.IsSuccess.Should().BeTrue();
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        fakeUser.BirthDate.Should().Be(birthDate);
    }
    
    [Theory]
    [InlineData(2030, 1, 1)]
    [InlineData(1899, 12, 31)]
    [InlineData(2024, 12, 1)]
    public async Task UpdateUserBirthDateCommandHandlerTest_Failure_WhenBirthDateValid(int year, int month, int day)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var birthDate = new DateTime(year, month, day);
        var command = new UpdateUserBirthDateCommand(fakeUser.Id, birthDate);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Never);
        
        result.IsFailure.Should().BeTrue();
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        fakeUser.BirthDate.Should().NotBe(birthDate);
    }
    
}