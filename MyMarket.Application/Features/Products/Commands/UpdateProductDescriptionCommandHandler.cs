using MyMarket.Application.Abstractions;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductDescriptionCommandHandler : ICommandHandler<UpdateProductDescriptionCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductDescriptionCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(UpdateProductDescriptionCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        
        if (product is null)
            throw new KeyNotFoundException("Product not found");
        
        product.UpdateDescription(command.Description);
        await _unitOfWork.CommitAsync();
    }
}