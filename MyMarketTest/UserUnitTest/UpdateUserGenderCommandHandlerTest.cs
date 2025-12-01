using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserGenderCommandHandlerTest
{
    private readonly UpdateUserGenderCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserGenderCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserGenderCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateUserGenderCommandHandlerTest_Success_WhenGenderIsValid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const Gender gender = Gender.Male;
        var command = new UpdateUserGenderCommand(fakeUser.Id, gender);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        fakeUser.Gender.Should().Be(gender);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(99)]
    [InlineData(1000)]
    public async Task UpdateUserGenderCommandHandlerTest_Failure_WhenGenderIsInvalid(int invalidGenderValue)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var gender = (Gender)invalidGenderValue;
        var command = new UpdateUserGenderCommand(fakeUser.Id, gender);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);

        var console = new UserConsoleOutPut();
        console.ConsoleOutput(fakeUser);
        
        result.IsFailure.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Never);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        fakeUser.Gender.Should().NotBe(gender);
    }
}