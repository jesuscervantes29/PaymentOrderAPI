using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    public Task<IEnumerable<Product>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Product>>(ProductSeed.Data);
}
