using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using ICommandHandler = ICommandHandler<UpdateUserBirthDateCommand, ResponseViewModel>;

public class UpdateUserBirthDateCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateUserBirthDateCommand command)
    {
        try
        {
            var product = await repository.GetByIdAsync(command.Id, CancellationToken.None);
            
            if(product is null)
                return  ResponseViewModel.Fail("Product not found", 404);
            
            product.UpdateBirthDate(command.BirthDate);
            
            await unitOfWork.CommitAsync(CancellationToken.None);

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return  ResponseViewModel.Fail(e.Message, 500);
        }
    }
}