using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

using ICommandHandler = ICommandHandler<UpdateProductSkuCommand, ResponseViewModel>;

public class UpdateProductSkuCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateProductSkuCommand command)
    {
        try
        {
            var product = await repository.GetByIdAsync(command.ProductId);
        
            if (product is null)
                return ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdateSku(command.Sku);
            await unitOfWork.CommitAsync();
            
            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}