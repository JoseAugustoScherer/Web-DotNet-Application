using MyMarket.Application.Abstractions;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductPriceCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(UpdateProductPriceCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        
        if (product is null)
            throw new KeyNotFoundException($"Product with id {command.Id} not found");
        
        product.UpdatePrice(command.Price);
        await _unitOfWork.CommitAsync();
    }
}