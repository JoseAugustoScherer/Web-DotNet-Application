using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

public class UpdateUserRoleCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserRoleCommand, ResponseViewModel>
{
    public async Task<ResponseViewModel> HandleAsync(UpdateUserRoleCommand command)
    {
        try
        {
            if (!Enum.IsDefined(typeof(Role), command.Role))
            {
                return ResponseViewModel.Fail("Invalid ROLE value", 404);
            }
            
            var product = await repository.GetByIdAsync(command.Id, CancellationToken.None);

            if (product is null)
                return ResponseViewModel.Fail($"User with ${command.Id}, not found", 404);

            product.UpdateRole(command.Role);

            await unitOfWork.CommitAsync(CancellationToken.None);

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}