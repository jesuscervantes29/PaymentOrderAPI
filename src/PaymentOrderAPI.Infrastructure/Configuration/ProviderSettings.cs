namespace PaymentOrderAPI.Infrastructure.Configuration;

internal class ProviderSettings
{
    public ProviderEntry PagaFacil { get; set; } = new();
    public ProviderEntry CazaPagos { get; set; } = new();
}

internal class ProviderEntry
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey  { get; set; } = string.Empty;
}
