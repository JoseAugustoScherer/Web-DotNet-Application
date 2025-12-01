using FluentAssertions;
using Moq;
using MyMarket.Application.Features.Products.Commands;
using MyMarket.Core.Repositories.Interfaces;
using MyMarketTest.Utils.ProductTest;

namespace MyMarketTest.ProductUnitTest;

public class UpdateProductDescriptionCommandHandlerTest
{
    private readonly UpdateProductDescriptionCommandHandler _sut;
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UpdateProductDescriptionCommandHandlerTest()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _sut = new UpdateProductDescriptionCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task UpdateProductDescriptionCommandHandlerTest_Success_WhenDescriptionIsValid()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const string newDescription = "New description";
        var command = new UpdateProductDescriptionCommand(fakeProduct.Id, newDescription);
        
        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsSuccess.Should().BeTrue();
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once());
        
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        fakeProduct.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public async Task UpdateProductDescriptionCommandHandlerTest_Failure_WhenDescriptionInvalid()
    {
        var fakeProduct = FakeDataProducts.FakeProductList(1).First();
        const string newDescription = "";
        var command = new UpdateProductDescriptionCommand(fakeProduct.Id, newDescription);
        
        _repository.Setup(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>())).ReturnsAsync(fakeProduct);
        
        var result = await _sut.HandleAsync(command);
        
        result.IsFailure.Should().BeTrue();
        
        _repository.Verify(p => p.GetByIdAsync(fakeProduct.Id, It.IsAny<CancellationToken>()), Times.Once);
    
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        fakeProduct.Description.Should().NotBe(newDescription);
    }
}