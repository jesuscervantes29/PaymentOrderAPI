using PaymentOrderAPI.Domain.Entities;

namespace PaymentOrderAPI.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
}
