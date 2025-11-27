using MyMarket.Application.Abstractions;
using MyMarket.Application.Features.Products.DTOs;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Queries;

public class GetProductByCategoryQueryHandler : IQueryHandler<GetProductByCategoryQuery, ResponseViewModel>
{
    private readonly IRepository<Product> _repository;

    public GetProductByCategoryQueryHandler(IRepository<Product> repository)
    {
        _repository = repository;
    }
    
    public async Task<ResponseViewModel> HandleAsync(GetProductByCategoryQuery query)
    {
        try
        {
            var allProducts = await _repository.GetAllAsync();

            var productsInCategory = allProducts.Where(p => p.Category == query.ProductCategory).ToList();

            var productResponse = productsInCategory.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Category,
                p.Price,
                p.Sku,
                p.Stock,
                p.CreatedOn
            )).ToList();

            return ResponseViewModel<List<ProductDto>>.Ok(productResponse);
        }
        catch (Exception e)
        {
            return ResponseViewModel<List<ProductDto>>.Fail(e.Message, 500);
        }
    }
}