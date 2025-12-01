using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.ProductTest;

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
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const decimal newPrice = 10.00m;
        var command = new UpdateProductPriceCommand(fakeProduct.Id, newPrice);
        
        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue("The operation should succeed");
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once());
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        fakeProduct.Price.Should().Be(newPrice, "The price should be updated");
    }
    
    [Fact]
    public async Task UpdateProductPriceCommandHandlerTest_Failure_WhenPriceIsNegative()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const decimal newPrice = -10.00m;
        var command = new UpdateProductPriceCommand(fakeProduct.Id, newPrice);
        
        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue("The price cannot be negative");
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once());
    
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        fakeProduct.Price.Should().NotBe(newPrice, "The price should be updated");
    }
}