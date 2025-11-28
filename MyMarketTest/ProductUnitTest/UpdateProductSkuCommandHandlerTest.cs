using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.ProductTest;

namespace MyMarketTest.ProductUnitTest;

public class UpdateProductSkuCommandHandlerTest
{
    private readonly UpdateProductSkuCommandHandler _sut;
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateProductSkuCommandHandlerTest()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateProductSkuCommandHandler(_repository.Object, _unitOfWork.Object);
    }
    
    [Fact]
    public async Task UpdateProductSkuCommandHandlerTest_Success_WhenSkuIsValid()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const string newSku = "sku";
        var command = new UpdateProductSkuCommand(fakeProduct.Id, newSku);
        
        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue("The operation should succeed");
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        fakeProduct.Sku.Should().Be(newSku, "The sku should be updated");
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateProductSkuCommandHandlerTest_Success_WhenSkuIsInvalid()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const string newSku = "";
        var command = new UpdateProductSkuCommand(fakeProduct.Id, newSku);
        
        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue("The Sku cannot be empty");
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}