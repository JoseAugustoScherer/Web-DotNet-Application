using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ResponseViewModel<Guid>>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateProductCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseViewModel<Guid>> HandleAsync(CreateProductCommand command)
    {
        try
        {
            var product = new Product(
                command.Name,
                command.Description,
                command.Category,
                command.Price,
                command.Sku,
                command.Stock);
        
            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            
            return ResponseViewModel<Guid>.Ok(product.Id);
        }
        catch (Exception e)
        {
            return ResponseViewModel<Guid>.Fail(e, 500);
        }
    }
}