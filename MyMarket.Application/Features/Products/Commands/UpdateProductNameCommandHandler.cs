using MyMarket.Application.Abstractions;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductNameCommandHandler : ICommandHandler<UpdateProductNameCommand>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateProductNameCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(UpdateProductNameCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);

        if (product is null)
            throw new KeyNotFoundException("Product not found");
        
        product.UpdateName(command.Name);
        await _unitOfWork.CommitAsync();
    }
}