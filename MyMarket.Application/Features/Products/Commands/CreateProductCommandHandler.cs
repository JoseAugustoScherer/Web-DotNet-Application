using MyMarket.Application.Abstractions;
using MyMarket.Application.Validators;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

using CreateHandler = ICommandHandler<CreateProductCommand, ResponseViewModel<Guid>>;

public class CreateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork, ProductValidator validator) : CreateHandler
{
    public async Task<ResponseViewModel<Guid>> HandleAsync(CreateProductCommand command)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(command);
            
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return ResponseViewModel<Guid>.Fail(errors, 400);
            }
            
            var product = new Product(
                command.Name,
                command.Description,
                command.ProductCategory,
                command.Price,
                command.Sku,
                command.Stock);
        
            await repository.AddAsync(product, null);
            await unitOfWork.CommitAsync(CancellationToken.None);
            
            return ResponseViewModel<Guid>.Ok(product.Id);
        }
        catch (Exception e)
        {
            return ResponseViewModel<Guid>.Fail(e.Message, 500);
        }
    }
}