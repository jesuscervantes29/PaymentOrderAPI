namespace PaymentOrderAPI.Application.Orders.Constants;

internal static class LogMessages
{
    public const string OrderCreated   = "[{Method}] Order created. Id: {Id} | Provider: {Provider} | Amount: {Amount}";
    public const string OrderCancelled = "[{Method}] Order cancelled. Id: {Id} | Provider: {Provider}";
    public const string OrderPaid      = "[{Method}] Order paid. Id: {Id} | Provider: {Provider}";
}
