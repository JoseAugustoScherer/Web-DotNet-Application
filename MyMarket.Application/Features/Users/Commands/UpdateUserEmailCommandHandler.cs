using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using ICommandHandler = ICommandHandler<UpdateUserEmailCommand, ResponseViewModel>;

public class UpdateUserEmailCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateUserEmailCommand command)
    {
        try
        {
            var product = await repository.GetByIdAsync(command.Id, CancellationToken.None);

            if (product is null)
                return ResponseViewModel.Fail("User not found", 404);

            product.UpdateEmail(command.Email);

            await unitOfWork.CommitAsync(CancellationToken.None);

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}