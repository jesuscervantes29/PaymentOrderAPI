using System.Text.Json.Serialization;

namespace PaymentOrderAPI.Infrastructure.Providers.Dtos;

internal record ProviderCreateRequest(
    [property: JsonPropertyName("method")]   string Method,
    [property: JsonPropertyName("products")] List<ProviderProductDto> Products
);

internal record ProviderProductDto(
    [property: JsonPropertyName("name")]      string Name,
    [property: JsonPropertyName("unitPrice")] decimal UnitPrice
);

internal record ProviderCreateResponse(
    [property: JsonPropertyName("orderId")]  string OrderId,
    [property: JsonPropertyName("amount")]   decimal Amount,
    [property: JsonPropertyName("fees")]     List<ProviderFeeDto> Fees,
    [property: JsonPropertyName("products")] List<ProviderProductDto> Products
);

internal record ProviderFeeDto(
    [property: JsonPropertyName("name")]   string? Name,
    [property: JsonPropertyName("title")]  string? Title,
    [property: JsonPropertyName("amount")] decimal Amount
)
{
    public string ResolvedName => Name ?? Title ?? string.Empty;
}
