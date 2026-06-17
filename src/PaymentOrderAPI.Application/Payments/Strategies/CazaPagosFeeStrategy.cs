using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Application.Payments.Strategies;

public class CazaPagosFeeStrategy : IProviderFeeStrategy
{
    private const string UnsupportedModeMessage = "CazaPagos does not support {0}.";

    public string ProviderName => "CazaPagos";

    public bool Supports(PaymentMode mode) =>
        mode == PaymentMode.TDC || mode == PaymentMode.Transfer;

    public decimal CalculateFee(PaymentMode mode, decimal totalAmount) => mode switch
    {
        PaymentMode.TDC      => CalculateTdcFee(totalAmount),
        PaymentMode.Transfer => CalculateTransferFee(totalAmount),
        _                    => throw new NotSupportedException(string.Format(UnsupportedModeMessage, mode))
    };

    private static decimal CalculateTdcFee(decimal amount) => amount switch
    {
        <= 1500m => amount * 0.02m,
        <= 5000m => amount * 0.015m,
        _        => amount * 0.005m
    };

    private static decimal CalculateTransferFee(decimal amount) => amount switch
    {
        <= 500m  => 5.00m,
        <= 1000m => amount * 0.025m,
        _        => amount * 0.02m
    };
}
