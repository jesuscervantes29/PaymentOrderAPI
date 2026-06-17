using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int orderId);
    Task<Order> UpdateStatusAsync(int orderId, OrderStatus status);
}
