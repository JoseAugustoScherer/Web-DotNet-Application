using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

using ICommandHandler = ICommandHandler<UpdateProductStockCommand, ResponseViewModel>;
public class UpdateProductStockCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateProductStockCommand command)
    {
        try
        {
            var product = await repository.GetByIdAsync(command.Id);
        
            if (product == null)
                return ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdateStock(command.Stock);
            await unitOfWork.CommitAsync();
            
            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return  ResponseViewModel.Fail(e.Message, 500);
        }
    }
}