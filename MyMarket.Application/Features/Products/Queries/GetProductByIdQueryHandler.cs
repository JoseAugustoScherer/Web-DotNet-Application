using MyMarket.Application.Abstractions;
using MyMarket.Application.Features.Products.DTOs;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ResponseViewModel>
{
    private readonly IRepository<Product> _repository;
    
    public GetProductByIdQueryHandler(IRepository<Product> repository)
    {
        _repository = repository;
    }
    
    public async Task<ResponseViewModel> HandleAsync(GetProductByIdQuery query)
    {
        try
        {
            var product = await _repository.GetByIdAsync(query.ProductId);

            if (product is null)
                return ResponseViewModel<ProductDto>.Fail("Product not found", 404);

            var productResponse = new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                product.Sku,
                product.Stock,
                product.CreatedOn);

            return ResponseViewModel<ProductDto>.Ok(productResponse);
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}