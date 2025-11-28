using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.ProductTest;

namespace MyMarketTest.ProductUnitTest;

public class UpdateProductNameCommandHandlerTest
{
    private readonly UpdateProductNameCommandHandler _sut;
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateProductNameCommandHandlerTest()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateProductNameCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateProductNameCommandHandlerTest_Success_WhenNameIsComplete()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const string newName = "Test";
        var command = new UpdateProductNameCommand(fakeProduct.Id, newName);

        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
    
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once());
        
        fakeProduct.Name.Should().Be(newName);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateProductNameCommandHandlerTest_Failure_WhenNameIsEmpty()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const string newName = "";
        var command = new UpdateProductNameCommand(fakeProduct.Id, newName);

        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue("The name cannot be empty");
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once);
    
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}