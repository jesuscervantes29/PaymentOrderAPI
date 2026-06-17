using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string ExternalOrderId { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public PaymentMode PaymentMode { get; set; }
    public List<Product> Products { get; set; } = new();
    public List<Fee> Fees { get; set; } = new();
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
