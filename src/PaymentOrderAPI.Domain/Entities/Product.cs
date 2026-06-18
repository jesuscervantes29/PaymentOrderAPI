namespace PaymentOrderAPI.Domain.Entities;

public class Product
{
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public decimal UnitPrice { get; set; }
}
