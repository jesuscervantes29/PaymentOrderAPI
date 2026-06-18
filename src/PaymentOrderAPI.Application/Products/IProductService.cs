using PaymentOrderAPI.Application.Orders.DTOs;

namespace PaymentOrderAPI.Application.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}
