using System.Text.Json.Serialization;

namespace PaymentOrderAPI.Infrastructure.Providers.Dtos;

internal record ProviderCreateRequest(
    [property: JsonPropertyName("PaymentMode")] string PaymentMode,
    [property: JsonPropertyName("Products")] List<ProviderProductDto> Products
);

internal record ProviderProductDto(
    [property: JsonPropertyName("Name")] string Name,
    [property: JsonPropertyName("UnitPrice")] decimal UnitPrice
);

internal record ProviderCreateResponse(
    [property: JsonPropertyName("OrderId")] string OrderId,
    [property: JsonPropertyName("Amount")] decimal Amount,
    [property: JsonPropertyName("Fees")] List<ProviderFeeDto> Fees,
    [property: JsonPropertyName("Products")] List<ProviderProductDto> Products
);

internal record ProviderFeeDto(
    [property: JsonPropertyName("Name")] string Name,
    [property: JsonPropertyName("Amount")] decimal Amount
);
