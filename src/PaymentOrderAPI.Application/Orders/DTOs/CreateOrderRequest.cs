namespace PaymentOrderAPI.Application.Orders.DTOs;

public record CreateOrderRequest(string PaymentMode, List<ProductDto> Products);
