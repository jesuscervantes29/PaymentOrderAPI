using PaymentOrderAPI.Application.Orders.DTOs;

namespace PaymentOrderAPI.Application.Orders;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    Task<IEnumerable<OrderResponse>> GetOrdersAsync();
    Task<OrderResponse> GetOrderByIdAsync(int orderId);
    Task CancelOrderAsync(int orderId);
    Task PayOrderAsync(int orderId);
}
