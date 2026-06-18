using PaymentOrderAPI.Application.Products.DTOs;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductCatalogDto>> GetProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(p => new ProductCatalogDto(p.Name, p.Details, p.IsAvailable, p.UnitPrice));
    }
}
