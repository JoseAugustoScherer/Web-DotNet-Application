using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Application.Validators;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.ProductTest;

namespace MyMarketTest.ProductUnitTest;

public class CreateProductCommandHandlerTest
{
    private readonly CreateProductCommandHandler _sut;
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ProductValidator _validator;

    public CreateProductCommandHandlerTest()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _validator = new ProductValidator();
        _sut = new CreateProductCommandHandler(_repository.Object, _unitOfWork.Object, _validator);
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct_WhenProductDoesNotExist()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
    
        var command = new CreateProductCommand(
            fakeProduct.Name,
            fakeProduct.Description,
            fakeProduct.Category,
            fakeProduct.Price,
            fakeProduct.Sku,
            fakeProduct.Stock);
        
        _repository.Setup(p => p.GetItemByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null!);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(r => r.AddAsync(
                It.Is<Product>(p => 
                    p.Name == fakeProduct.Name &&
                    p.Description == fakeProduct.Description &&
                    p.Category == fakeProduct.Category &&
                    p.Price == fakeProduct.Price &&
                    p.Sku == fakeProduct.Sku &&
                    p.Stock == fakeProduct.Stock
                ), It.IsAny<CancellationToken>()), 
            Times.Once);
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}