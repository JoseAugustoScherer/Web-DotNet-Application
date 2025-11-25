using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductStockCommandHandler : ICommandHandler<UpdateProductStockCommand, ResponseViewModel>
{
    private readonly IRepository<Product> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductStockCommandHandler(IRepository<Product> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseViewModel> HandleAsync(UpdateProductStockCommand command)
    {
        try
        {
            var product = await _repository.GetByIdAsync(command.Id);
        
            if (product == null)
                return ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdateStock(command.Stock);
            await _unitOfWork.CommitAsync();
            
            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return  ResponseViewModel.Fail(e.Message, 500);
        }
    }
}