using PaymentOrderAPI.Application.Payments.Strategies;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.UnitTests.Payments;

public class PagaFacilFeeStrategyTests
{
    private readonly PagaFacilFeeStrategy _sut = new();

    [Theory]
    [InlineData(PaymentMode.Cash, 500,     true)]
    [InlineData(PaymentMode.TDC,0,      true)]
    [InlineData(PaymentMode.Transfer,0, false)]
    public void Supports_ReturnsExpected(PaymentMode mode, decimal amount , bool expected)
    {
        Assert.Equal(expected, _sut.Supports(mode, amount));
    }

    [Fact]
    public void CalculateFee_Cash_Returns15Fixed()
    {
        var fee = _sut.CalculateFee(PaymentMode.Cash, 9999m);

        Assert.Equal(15.00m, fee);
    }

    [Theory]
    [InlineData(200,   2.00)]
    [InlineData(1000, 10.00)]
    [InlineData(6000, 60.00)]
    public void CalculateFee_TDC_Returns1Percent(decimal amount, decimal expected)
    {
        Assert.Equal(expected, _sut.CalculateFee(PaymentMode.TDC, amount));
    }

    [Fact]
    public void CalculateFee_UnsupportedMode_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            _sut.CalculateFee(PaymentMode.Transfer, 1000m));
    }
}
