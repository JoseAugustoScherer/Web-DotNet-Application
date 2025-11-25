using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ResponseViewModel>
{
    private readonly IProductRepository _productRepository;
    
    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ResponseViewModel> HandleAsync(GetProductByIdQuery query)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(query.ProductId);

            if (product is null)
                return ResponseViewModel<ProductDTO>.Fail("Product not found", 404);

            var productResponse = new ProductDTO(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                product.Sku,
                product.Stock);

            return ResponseViewModel<ProductDTO>.Ok(productResponse);
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
    }
}