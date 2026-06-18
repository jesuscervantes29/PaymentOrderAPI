using PaymentOrderAPI.Application.Products.DTOs;

namespace PaymentOrderAPI.Application.Products;

public interface IProductService
{
    Task<IEnumerable<ProductCatalogDto>> GetProductsAsync();
}
