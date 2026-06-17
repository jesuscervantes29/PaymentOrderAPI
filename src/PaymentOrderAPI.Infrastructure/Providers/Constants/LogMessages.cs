namespace PaymentOrderAPI.Infrastructure.Providers.Constants;

internal static class LogMessages
{
    public const string CreatingOrder   = "[{Method}] Creating order. Mode: {Mode}";
    public const string CancellingOrder = "[{Method}] Cancelling order {OrderId}";
    public const string PayingOrder     = "[{Method}] Paying order {OrderId}";
}
