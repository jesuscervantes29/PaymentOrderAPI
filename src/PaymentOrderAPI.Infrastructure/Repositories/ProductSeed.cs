using PaymentOrderAPI.Domain.Entities;

namespace PaymentOrderAPI.Infrastructure.Repositories;

internal static class ProductSeed
{
    public static IReadOnlyList<Product> Data { get; } = new List<Product>
    {
        new() { Name = "Dell Inspiron 5050",  Details = "32Gb / 512 SSD / i7 2.2Ghz",       IsAvailable = true,  UnitPrice = 15000m },
        new() { Name = "Lenovo ThinkPad X1",  Details = "16Gb / 256 SSD / i5 1.8Ghz",       IsAvailable = true,  UnitPrice =  4500m },
        new() { Name = "Monitor LG 27\"",     Details = "4K UHD / 144Hz / IPS",             IsAvailable = true,  UnitPrice =  1200m },
        new() { Name = "Teclado Mecánico",    Details = "Cherry MX / RGB / TKL",            IsAvailable = true,  UnitPrice =   800m },
        new() { Name = "Webcam Logitech",     Details = "1080p / 60fps / Autofocus",        IsAvailable = true,  UnitPrice =   650m },
        new() { Name = "Headset 450p",        Details = "Logitech / Active Noise Can",      IsAvailable = false, UnitPrice =  1800m },
        new() { Name = "SSD Samsung 1TB",     Details = "NVMe / 3500 MB/s / M.2",          IsAvailable = true,  UnitPrice =  2200m },
        new() { Name = "Memoria RAM 16GB",    Details = "DDR5 / 5600MHz / Kingston",        IsAvailable = true,  UnitPrice =   950m },
    };
}
