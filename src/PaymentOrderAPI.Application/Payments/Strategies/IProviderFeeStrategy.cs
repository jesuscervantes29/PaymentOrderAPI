using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Application.Payments.Strategies;

public interface IProviderFeeStrategy
{
    string ProviderName { get; }
    bool Supports(PaymentMode mode, decimal? amount);
    decimal CalculateFee(PaymentMode mode, decimal totalAmount);
}
