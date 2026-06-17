namespace PaymentOrderAPI.Application.Orders.DTOs;

public record OrderResponse(
    int Id,
    string ExternalOrderId,
    string ProviderName,
    string Status,
    string PaymentMode,
    List<ProductDto> Products,
    List<FeeDto> Fees,
    decimal Amount,
    DateTime CreatedAt
);
