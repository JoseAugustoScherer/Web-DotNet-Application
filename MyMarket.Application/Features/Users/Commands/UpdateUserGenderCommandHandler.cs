using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Enums;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Users.Commands;

using ICommandHandler = ICommandHandler<UpdateUserGenderCommand, ResponseViewModel>;

public class UpdateUserGenderCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork) : ICommandHandler
{
    public async Task<ResponseViewModel> HandleAsync(UpdateUserGenderCommand command)
    {
        try
        {
            if (!Enum.IsDefined(typeof(Gender), command.Gender))
            {
                return ResponseViewModel.Fail("Invalid GENDER value", 404);
            }
            
            var product = await repository.GetByIdAsync(command.Id, CancellationToken.None);
            
            if (product is null)
                return ResponseViewModel.Fail($"Product with id {command.Id} not found", 404);
            
            product.UpdateGender(command.Gender);
            
            await unitOfWork.CommitAsync(CancellationToken.None);

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}