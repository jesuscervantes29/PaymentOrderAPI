using PaymentOrderAPI.Domain.Entities;

namespace PaymentOrderAPI.Domain.Interfaces;

public record ProviderOrderResult(
    string ExternalOrderId,
    decimal Amount,
    IEnumerable<Fee> Fees
);
