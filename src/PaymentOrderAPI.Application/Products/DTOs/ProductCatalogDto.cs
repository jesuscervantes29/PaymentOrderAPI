namespace PaymentOrderAPI.Application.Products.DTOs;

public record ProductCatalogDto(string Name, string Details, bool IsAvailable, decimal UnitPrice);
