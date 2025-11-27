using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

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
        var productId =  Guid.NewGuid();
        const string newName = "Test";
        var command = new UpdateProductNameCommand(productId, newName);

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
        
        result.IsSuccess.Should().BeTrue();
    
        _repository
            .Verify(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()), 
                Times.Once());
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateProductNameCommandHandlerTest_Failure_WhenNameIsEmpty()
    {
        var productId =  Guid.NewGuid();
        const string newName = "";
        var command = new UpdateProductNameCommand(productId, newName);

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
        
        result.IsFailure.Should().BeTrue("The name cannot be empty");
        
        _repository.Verify(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken?>()), Times.Once);
    
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}