using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class UpdateProductNameCommandHandler : ICommandHandler<UpdateProductNameCommand, ResponseViewModel>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateProductNameCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseViewModel> HandleAsync(UpdateProductNameCommand command)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(command.Id);

            if (product is null)
                ResponseViewModel.Fail("Product not found", 404);
        
            product.UpdateName(command.Name);
            await _unitOfWork.CommitAsync();

            return ResponseViewModel.Ok();
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}