using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Queries;

public class GetProductByCategoryQueryHandler : IQueryHandler<GetProductByCategoryQuery, ResponseViewModel<List<ProductDTO>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByCategoryQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ResponseViewModel<List<ProductDTO>>> HandleAsync(GetProductByCategoryQuery query)
    {
        var allProducts = await _productRepository.GetAllAsync();
        
        var productsInCategory = allProducts.Where(p => p.Category == query.ProductCategory).ToList();
        
        var productResponse = productsInCategory.Select(product => new ProductDTO(
            product.Id,
            product.Name,
            product.Description,
            product.Category,
            product.Price,
            product.Sku,
            product.Stock
            )).ToList();
        
        return ResponseViewModel<List<ProductDTO>>.Ok(productResponse);
    }
}