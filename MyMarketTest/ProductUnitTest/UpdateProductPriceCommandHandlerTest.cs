using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarketTest.ProductUnitTest;

public class UpdateProductPriceCommandHandlerTest
{
    private readonly UpdateProductPriceCommandHandler _sut;
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateProductPriceCommandHandlerTest()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateProductPriceCommandHandler(_repository.Object, _unitOfWork.Object);
    }
    
    [Fact]
    public async Task UpdateProductPriceCommandHandlerTest_Success_WhenPriceIsPositive()
    {
        var productId = Guid.NewGuid();
        var newPrice = 10.00m;
        var command = new UpdateProductPriceCommand(productId, newPrice);

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
        product.Price.Should().Be(newPrice, "The price should be updated");
        
        _repository
            .Verify(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()), 
                Times.Once());
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateProductPriceCommandHandlerTest_Failure_WhenPriceIsNegative()
    {
        var productId = Guid.NewGuid();
        const decimal invalidPrice = -10.00m;
        var command = new UpdateProductPriceCommand(productId, invalidPrice);

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
        
        result.IsFailure.Should().BeTrue("The price cannot be negative");
        
        _repository.Verify(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()), Times.Once());
    
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}