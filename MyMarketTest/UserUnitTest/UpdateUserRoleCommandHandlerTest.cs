using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Users.Commands;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.UserTest;

namespace MyMarketTest.UserUnitTest;

public class UpdateUserRoleCommandHandlerTest
{
    private readonly UpdateUserRoleCommandHandler _sut;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateUserRoleCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateUserRoleCommandHandler(_userRepository.Object, _unitOfWork.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task UpdateUserRoleCommand_Success_WhenRoleIsValid(int validRoleOptions)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var role = (Role)validRoleOptions;
        var command = new UpdateUserRoleCommand(fakeUser.Id, role);
        
        _userRepository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);
        
        _userRepository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        result.IsSuccess.Should().BeTrue();
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Once);
        
        fakeUser.Role.Should().Be(role);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(-2)]
    [InlineData(32)]
    public async Task UpdateUserRoleCommand_Failure_WhenRoleIsInvalid(int invalidRoleOptions)
    {
        var fakeUser = FakeDataUser.FakeUserList(1).First();
        var role = (Role)invalidRoleOptions;
        var command = new UpdateUserRoleCommand(fakeUser.Id, role);
        
        _userRepository.Setup(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeUser);

        var result = await _sut.HandleAsync(command);
        
        _userRepository.Verify(r => r.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()), Times.Never);
        
        result.IsFailure.Should().BeTrue();
        
        _unitOfWork.Verify(u => u.CommitAsync(CancellationToken.None), Times.Never);
        
        fakeUser.Role.Should().NotBe(role);
    }
}