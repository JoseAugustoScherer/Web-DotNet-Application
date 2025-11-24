using MyMarket.Application.Abstractions;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductStockCommandHandler : ICommandHandler<UpdateProductStockCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductStockCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(UpdateProductStockCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        
        if (product == null)
            throw new KeyNotFoundException($"Product with id {command.Id} not found");
        
        product.UpdateStock(command.Stock);
        await _unitOfWork.CommitAsync();
    }
}