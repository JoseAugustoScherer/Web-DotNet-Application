using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

using ICommandHandler = ICommandHandler<UpdateProductDescriptionCommand, ResponseViewModel>;

public class UpdateProductDescriptionCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateProductDescriptionCommand command)
    {
        try
        {
            var product = await repository.GetByIdAsync(command.Id);
        
            if (product is null)
                return ResponseViewModel.Fail("Product not found", 404);
            
            product.UpdateDescription(command.Description);
            await unitOfWork.CommitAsync();

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
        
    }
}