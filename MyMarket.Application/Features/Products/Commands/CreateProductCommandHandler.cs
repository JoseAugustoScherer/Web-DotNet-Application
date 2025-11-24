using MyMarket.Application.Abstractions;
using MyMarket.Core.Entities;
using MyMarket.Core.Repositories.Interfaces;

namespace MyMarket.Application.Features.Products.Commands;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateProductCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(CreateProductCommand command)
    {
        var product = new Product(
            command.Name,
            command.Description,
            command.Category,
            command.Price,
            command.Sku,
            command.Stock);
        
        await _productRepository.AddAsync(product);
        
        await _unitOfWork.CommitAsync(); 
        
        return product.Id;
    }
}

// ========================================================================================================================================================= //
// TODO 1. Para validar atributos usar o fluent validation, colocando a cargo do service a validação dos atributos e não do objeto - pesquisar no google;    //
// TODO 2. Implementar o response view model pattern - falar com o Paulo;                                                                                    //
// TODO 3. Criar um handler generico para guiar a implementação dos handlres normais;                                                                        //
// ========================================================================================================================================================= //