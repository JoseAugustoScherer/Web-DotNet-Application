using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductStockCommandHandler : ICommandHandler<UpdateProductStockCommand, ResponseViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductStockCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseViewModel> HandleAsync(UpdateProductStockCommand command)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(command.Id);
        
            if (product == null)
                return ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdateStock(command.Stock);
            await _unitOfWork.CommitAsync();
            
            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return  ResponseViewModel.Fail(e, 500);
        }
    }
}