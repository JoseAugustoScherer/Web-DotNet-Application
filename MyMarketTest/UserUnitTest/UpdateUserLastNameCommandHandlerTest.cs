using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserLastNameCommandHandlerTest
{
    private readonly UpdateUserLastNameCommandHandler _sut;
    private readonly Mock<IUserRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    
    public UpdateUserLastNameCommandHandlerTest()
    {
        _repository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserLastNameCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateUserLastNameCommandHandlerTest_Success_WhenLastNameIsValid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const string newLastName = "newLastName";
        var command = new UpdateUserLastNameCommand(fakeUser.Id, newLastName);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        fakeUser.LastName.Should().Be(newLastName);
    }
    
    [Fact]
    public async Task UpdateUserLastNameCommandHandlerTest_Success_WhenLastNameIsInvalid()
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        const string newLastName = "";
        var command = new UpdateUserLastNameCommand(fakeUser.Id, newLastName);
        
        _repository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue();
        
        _repository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        fakeUser.LastName.Should().NotBe(newLastName);
    }
} 