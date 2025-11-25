using MyMarket.Application.Abstractions;
using MyMarket.Application.ViewModel;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Queries;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, ResponseViewModel>
{
    private readonly IRepository<Product> _productRepository;
    
    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ResponseViewModel> HandleAsync(GetAllProductsQuery query)
    {
        try
        {
            var products = await _productRepository.GetAllAsync();

            var productsDto = products.Select(p => new ProductDTO(
                p.Id,
                p.Name,
                p.Description,
                p.Category,
                p.Price,
                p.Sku,
                p.Stock)).ToList();
        
            return ResponseViewModel<List<ProductDTO>>.Ok(productsDto);
        }
        catch (Exception e)
        {
            return ResponseViewModel.Fail(e.Message, 500);
        }
        
    }
}