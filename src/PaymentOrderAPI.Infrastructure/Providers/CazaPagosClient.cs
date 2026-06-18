using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;
using PaymentOrderAPI.Domain.Interfaces;
using PaymentOrderAPI.Infrastructure.Providers.Constants;
using PaymentOrderAPI.Infrastructure.Providers.Dtos;

namespace PaymentOrderAPI.Infrastructure.Providers;

public class CazaPagosClient : IPaymentProviderClient
{
    private const string CancelRoute = "cancellation";
    private const string PayRoute    = "payment";

    private readonly HttpClient _httpClient;
    private readonly ILogger<CazaPagosClient> _logger;

    public string ProviderName => "CazaPagos";

    public CazaPagosClient(HttpClient httpClient, ILogger<CazaPagosClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProviderOrderResult> CreateOrderAsync(PaymentMode mode, IEnumerable<Product> products)
    {
        var request = new ProviderCreateRequest(
            MapPaymentMode(mode),
            products.Select(p => new ProviderProductDto(p.Name, p.UnitPrice)).ToList()
        );

        _logger.LogInformation(LogMessages.CreatingOrder, nameof(CreateOrderAsync), mode);

        var response = await _httpClient.PostAsJsonAsync(ProviderRoutes.CreateOrder, request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ProviderCreateResponse>();

        return new ProviderOrderResult(
            result!.OrderId,
            result.Amount,
            result.Fees.Select(f => new Fee { Name = f.ResolvedName, Amount = f.Amount })
        );
    }

    public async Task CancelOrderAsync(string externalOrderId)
    {
        _logger.LogInformation(LogMessages.CancellingOrder, nameof(CancelOrderAsync), externalOrderId);

        var response = await _httpClient.PutAsync($"{CancelRoute}?id={externalOrderId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task PayOrderAsync(string externalOrderId)
    {
        _logger.LogInformation(LogMessages.PayingOrder, nameof(PayOrderAsync), externalOrderId);

        var response = await _httpClient.PutAsync($"{PayRoute}?id={externalOrderId}", null);
        response.EnsureSuccessStatusCode();
    }

    private static string MapPaymentMode(PaymentMode mode) => mode switch
    {
        PaymentMode.TDC      => "CreditCard",
        PaymentMode.Transfer => "Transfer",
        _                    => throw new NotSupportedException($"CazaPagos does not support {mode}.")
    };
}
