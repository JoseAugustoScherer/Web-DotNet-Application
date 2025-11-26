using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Application.Validators;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

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
        var validator = new ProductValidator();
        _sut = new CreateProductCommandHandler(_repository.Object, _unitOfWork.Object, _validator);
    }

    [Fact]
    public async Task FailCreate()
    {
        var command = new CreateProductCommand("", "Description", Category.Automotive, 19.99m, "SKU", 19);

        var product = new Product("", "Description", Category.Automotive, 19.99m, "SKU", 19);

        _repository
            .Setup(p => p.GetItemByAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        
        var result = await _sut.HandleAsync(command);
        
        Assert.True(result.IsFailure);
    }
}