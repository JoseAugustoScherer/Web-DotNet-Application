using MyMarket.Application.Abstractions;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductSkuCommandHandler : ICommandHandler<UpdateProductSkuCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductSkuCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(UpdateProductSkuCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);
        
        if (product is null)
            throw new KeyNotFoundException("Product not found");
        
        product.UpdateSku(command.Sku);
        await _unitOfWork.CommitAsync();
    }
}