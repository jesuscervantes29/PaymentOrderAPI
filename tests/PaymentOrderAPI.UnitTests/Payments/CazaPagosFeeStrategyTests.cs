using PaymentOrderAPI.Application.Payments.Strategies;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.UnitTests.Payments;

public class CazaPagosFeeStrategyTests
{
    private readonly CazaPagosFeeStrategy _sut = new();

    [Theory]
    [InlineData(PaymentMode.TDC, 0,      true)]
    [InlineData(PaymentMode.Transfer, 0, true)]
    [InlineData(PaymentMode.Cash, 0,     false)]
    public void Supports_ReturnsExpected(PaymentMode mode, decimal amount, bool expected)
    {
        Assert.Equal(expected, _sut.Supports(mode, amount));
    }

    [Theory]
    [InlineData(500,   10.00)]  // <= 1500 → 2%
    [InlineData(1500,  30.00)]  // <= 1500 → 2%
    [InlineData(2000,  30.00)]  // <= 5000 → 1.5%
    [InlineData(5000,  75.00)]  // <= 5000 → 1.5%
    [InlineData(6000,  30.00)]  // >  5000 → 0.5%
    public void CalculateFee_TDC_ReturnsExpectedByTier(decimal amount, decimal expected)
    {
        Assert.Equal(expected, _sut.CalculateFee(PaymentMode.TDC, amount));
    }

    [Theory]
    [InlineData(300,   5.00)]   // <= 500  → $5 fijo
    [InlineData(500,   5.00)]   // <= 500  → $5 fijo
    [InlineData(750,  18.75)]   // <= 1000 → 2.5%
    [InlineData(1000, 25.00)]   // <= 1000 → 2.5%
    [InlineData(2000, 40.00)]   // >  1000 → 2%
    public void CalculateFee_Transfer_ReturnsExpectedByTier(decimal amount, decimal expected)
    {
        Assert.Equal(expected, _sut.CalculateFee(PaymentMode.Transfer, amount));
    }

    [Fact]
    public void CalculateFee_UnsupportedMode_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            _sut.CalculateFee(PaymentMode.Cash, 1000m));
    }
}
