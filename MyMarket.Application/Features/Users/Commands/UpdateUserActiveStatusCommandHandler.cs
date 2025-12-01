using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using ICommandHandler = ICommandHandler<UpdateUserActiveStatusCommand, ResponseViewModel>;

public class UpdateUserActiveStatusCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateUserActiveStatusCommand command)
    {
        try
        {
            if (!Enum.IsDefined(typeof(ActiveStatus), command.ActiveStatus))
            {
                return ResponseViewModel.Fail("Invalid ACTIVE STATUS value", 404);
            }
            
            var product = await repository.GetByIdAsync(command.Id, CancellationToken.None);

            if (product is null)
                return ResponseViewModel.Fail($"User with ${command.Id}, not found.", 404);

            product.UpdateStatus(command.ActiveStatus);

            await unitOfWork.CommitAsync(CancellationToken.None);

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}