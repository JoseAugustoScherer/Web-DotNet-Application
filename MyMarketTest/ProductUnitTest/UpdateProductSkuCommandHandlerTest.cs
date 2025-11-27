using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

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
        var productId = Guid.NewGuid();
        const string newSku = "sku";
        var command = new UpdateProductSkuCommand(productId, newSku);

        var product = new Product(
            "Test",
            "Description",
            Category.Automotive,
            19.99m,
            "SKU",
            19);
        
        _repository
            .Setup(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()))
            .ReturnsAsync(product);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue("The operation should succeed");
        product.Sku.Should().Be(newSku, "The sku should be updated");
        
        _repository.Verify(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateProductSkuCommandHandlerTest_Success_WhenSkuIsInvalid()
    {
        var productId = Guid.NewGuid();
        const string newSku = "";
        var command = new UpdateProductSkuCommand(productId, newSku);

        var product = new Product(
            "Test",
            "Description",
            Category.Automotive,
            19.99m,
            "SKU",
            19);
        
        _repository
            .Setup(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()))
            .ReturnsAsync(product);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue("The Sku cannot be empty");
        
        _repository.Verify(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()), Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}