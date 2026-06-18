using Microsoft.Extensions.Logging.Abstractions;
using PaymentOrderAPI.Application.Payments;
using PaymentOrderAPI.Application.Payments.Strategies;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.UnitTests.Payments;

public class PaymentProviderSelectorTests
{
    private readonly PaymentProviderSelector _sut;

    public PaymentProviderSelectorTests()
    {
        var strategies = new IProviderFeeStrategy[]
        {
            new PagaFacilFeeStrategy(),
            new CazaPagosFeeStrategy()
        };

        _sut = new PaymentProviderSelector(strategies, NullLogger<PaymentProviderSelector>.Instance);
    }

    [Fact]
    public void SelectOptimal_Cash_ReturnsPagaFacil()
    {
        var result = _sut.SelectOptimal(PaymentMode.Cash, 1000m);

        Assert.Equal("PagaFacil", result.ProviderName);
    }

    [Fact]
    public void SelectOptimal_Transfer_ReturnsCazaPagos()
    {
        var result = _sut.SelectOptimal(PaymentMode.Transfer, 1000m);

        Assert.Equal("CazaPagos", result.ProviderName);
    }

    [Theory]
    [InlineData(200,   "PagaFacil")]  // PF: $2.00  vs CP: $4.00  (2%)
    [InlineData(1000,  "PagaFacil")]  // PF: $10.00 vs CP: $20.00 (2%)
    [InlineData(2000,  "PagaFacil")]  // PF: $20.00 vs CP: $30.00 (1.5%)
    [InlineData(6000,  "CazaPagos")]  // PF: $60.00 vs CP: $30.00 (0.5%)
    public void SelectOptimal_TDC_ReturnsProviderWithLowestFee(decimal amount, string expectedProvider)
    {
        var result = _sut.SelectOptimal(PaymentMode.TDC, amount);

        Assert.Equal(expectedProvider, result.ProviderName);
    }

    [Fact]
    public void SelectOptimal_NoProviderSupportsMode_ThrowsNotSupportedException()
    {
        var sut = new PaymentProviderSelector(
            new[] { new PagaFacilFeeStrategy() },
            NullLogger<PaymentProviderSelector>.Instance);

        Assert.Throws<NotSupportedException>(() =>
            sut.SelectOptimal(PaymentMode.Transfer, 1000m));
    }
}
