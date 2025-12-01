using System.Linq.Expressions;
using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

using ICommandHandler = ICommandHandler<DeleteProductCommand, ResponseViewModel>;

public class DeleteProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(DeleteProductCommand command)
    {
        try
        {
            var product = await repository.GetItemByAsync(x => x.Id == command.Id, CancellationToken.None);
            
            if (product is null)
                return ResponseViewModel.Fail("Product not found", 404);
            
            await repository.DeleteAsync(product, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 404);
        }
    }
}