using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductSkuCommandHandler : ICommandHandler<UpdateProductSkuCommand, ResponseViewModel>
{
    private readonly IRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductSkuCommandHandler(IRepository<Product> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseViewModel> HandleAsync(UpdateProductSkuCommand command)
    {
        try
        {
            var product = await _repository.GetByIdAsync(command.ProductId);
        
            if (product is null)
                return ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdateSku(command.Sku);
            await _unitOfWork.CommitAsync();
            
            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}