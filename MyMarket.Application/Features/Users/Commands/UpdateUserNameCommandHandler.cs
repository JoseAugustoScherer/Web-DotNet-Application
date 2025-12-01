using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using ICommandHandler = ICommandHandler<UpdateUserNameCommand, ResponseViewModel>;

public class UpdateUserNameCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateUserNameCommand command)
    {
        try
        {
            var product = await repository.GetByIdAsync(command.Id, CancellationToken.None);
            
            if (product is null)
                return ResponseViewModel.Fail("Product not found", 404);
            
            product.UpdateName(command.Name);
            
            await unitOfWork.CommitAsync(CancellationToken.None);
            
            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}