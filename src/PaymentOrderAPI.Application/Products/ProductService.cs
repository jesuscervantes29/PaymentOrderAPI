using PaymentOrderAPI.Application.Orders.DTOs;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(p => new ProductDto(p.Name, p.UnitPrice));
    }
}
