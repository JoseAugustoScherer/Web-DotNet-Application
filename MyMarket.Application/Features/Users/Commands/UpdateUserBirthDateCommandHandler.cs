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
            if(command.BirthDate > DateTime.Now)
                return ResponseViewModel.Fail($"Birth date {command.BirthDate} is in the future", 400);
            if (command.BirthDate >  DateTime.Now.AddYears(-18))
                return ResponseViewModel.Fail("User must be at least 18 year", 400);
            if (command.BirthDate.Year < 1900)
                return ResponseViewModel.Fail("Are you a Vampire? I don't think so.", 400);
            
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