using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Application.Payments.Strategies;

public class PagaFacilFeeStrategy : IProviderFeeStrategy
{
    private const string UnsupportedModeMessage = "PagaFacil does not support {0}.";

    public string ProviderName => "PagaFacil";

    public bool Supports(PaymentMode mode) =>
        mode == PaymentMode.Cash || mode == PaymentMode.TDC;

    public decimal CalculateFee(PaymentMode mode, decimal totalAmount) => mode switch
    {
        PaymentMode.Cash => 15.00m,
        PaymentMode.TDC  => totalAmount * 0.01m,
        _                => throw new NotSupportedException(string.Format(UnsupportedModeMessage, mode))
    };
}
