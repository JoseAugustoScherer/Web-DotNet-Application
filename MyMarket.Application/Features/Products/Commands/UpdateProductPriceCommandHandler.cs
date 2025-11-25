using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand, ResponseViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductPriceCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseViewModel> HandleAsync(UpdateProductPriceCommand command)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(command.Id);
        
            if (product is null)
                return ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdatePrice(command.Price);
            await _unitOfWork.CommitAsync();

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}