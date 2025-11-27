using System.Linq.Expressions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarketTest.ProductUnitTest;

public class UpdateProductCategoryCommandHandlerTest
{
    private readonly UpdateProductCategoryCommandHandler _sut;
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateProductCategoryCommandHandlerTest()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateProductCategoryCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task FailUpdateProductCategoryCommand()
    {
        var productId = Guid.NewGuid();
        var command = new UpdateProductCategoryCommand(productId, (Category)999);

        var product = new Product(
            "Test", 
            "Description", 
            Category.Automotive, 
            19.99m, 
            "SKU", 
            19);
        
        _repository
            .Setup(p => p.GetItemByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        
        var result = await _sut.HandleAsync(command);
        
        Assert.True(result.IsFailure);
    }
}