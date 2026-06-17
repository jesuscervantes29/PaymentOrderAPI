using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Domain.Interfaces;

public interface IPaymentProviderClient
{
    string ProviderName { get; }
    Task<ProviderOrderResult> CreateOrderAsync(PaymentMode mode, IEnumerable<Product> products);
    Task CancelOrderAsync(string externalOrderId);
    Task PayOrderAsync(string externalOrderId);
}
