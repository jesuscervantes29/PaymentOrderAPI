using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;
using PaymentOrderAPI.Domain.Interfaces;
using PaymentOrderAPI.Infrastructure.Providers.Constants;
using PaymentOrderAPI.Infrastructure.Providers.Dtos;

namespace PaymentOrderAPI.Infrastructure.Providers;

public class PagaFacilClient : IPaymentProviderClient
{
    private const string CancelRoute = "cancel";
    private const string PayRoute    = "pay";

    private readonly HttpClient _httpClient;
    private readonly ILogger<PagaFacilClient> _logger;

    public string ProviderName => "PagaFacil";

    public PagaFacilClient(HttpClient httpClient, ILogger<PagaFacilClient> logger)
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
            result.Fees.Select(f => new Fee { Name = f.Name, Amount = f.Amount })
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
        PaymentMode.Cash     => "Cash",
        PaymentMode.TDC      => "Card",
        PaymentMode.Transfer => "Transfer",
        _                    => throw new NotSupportedException($"PagaFacil does not support {mode}.")
    };
}
