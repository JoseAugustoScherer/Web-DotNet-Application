using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserNameCommandHandlerTest
{
    private readonly UpdateUserNameCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserNameCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserNameCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateUserNameCommandHandlerTest_Success_WhenNameIsComplete()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const string newName = "Test";
        var command = new UpdateUserNameCommand(fakeUser.Id, newName);
        
        _repository.Setup(p => p.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(p => p.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        fakeUser.Name.Should().Be(newName);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserNameCommandHandlerTest_Failure_WhenNameIsEmpty()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const string newName = "";
        var command = new UpdateUserNameCommand(fakeUser.Id, newName);
        
        _repository.Setup(p => p.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeFalse();
        
        _repository.Verify(p => p.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        fakeUser.Name.Should().NotBe(newName);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}