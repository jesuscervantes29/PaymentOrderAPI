using PaymentOrderAPI.Domain.Entities;

namespace PaymentOrderAPI.Infrastructure.Repositories;

internal static class ProductSeed
{
    public static IReadOnlyList<Product> Data { get; } = new List<Product>
    {
        new() { Name = "Laptop Pro",          UnitPrice = 15000m },
        new() { Name = "Monitor 27\"",         UnitPrice =  4500m },
        new() { Name = "Teclado Mecánico",    UnitPrice =  1200m },
        new() { Name = "Mouse Inalámbrico",   UnitPrice =   800m },
        new() { Name = "Webcam HD",           UnitPrice =   650m },
        new() { Name = "Audífonos",           UnitPrice =  1800m },
        new() { Name = "SSD 1TB",             UnitPrice =  2200m },
        new() { Name = "Memoria RAM 16GB",    UnitPrice =   950m },
    };
}
