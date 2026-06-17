using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private const string OrderNotFoundMessage = "Order {0} not found.";

    private readonly Dictionary<int, Order> _store = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public Task<Order> CreateAsync(Order order)
    {
        lock (_lock)
        {
            order.Id = _nextId++;
            order.CreatedAt = DateTime.UtcNow;
            _store[order.Id] = order;
            return Task.FromResult(order);
        }
    }

    public Task<IEnumerable<Order>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IEnumerable<Order>>(_store.Values.ToList());
        }
    }

    public Task<Order?> GetByIdAsync(int orderId)
    {
        lock (_lock)
        {
            _store.TryGetValue(orderId, out var order);
            return Task.FromResult(order);
        }
    }

    public Task<Order> UpdateStatusAsync(int orderId, OrderStatus status)
    {
        lock (_lock)
        {
            if (!_store.TryGetValue(orderId, out var order))
                throw new KeyNotFoundException(string.Format(OrderNotFoundMessage, orderId));

            order.Status = status;
            return Task.FromResult(order);
        }
    }
}
