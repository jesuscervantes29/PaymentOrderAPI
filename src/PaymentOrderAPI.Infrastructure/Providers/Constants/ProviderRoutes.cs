namespace PaymentOrderAPI.Infrastructure.Providers.Constants;

internal static class ProviderRoutes
{
    public const string CreateOrder  = "api/orders";
    public const string CancelOrder  = "api/orders/{0}/cancel";
    public const string PayOrder     = "api/orders/{0}/pay";
}
