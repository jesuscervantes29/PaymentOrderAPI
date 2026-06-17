using Microsoft.Extensions.Logging;
using PaymentOrderAPI.Application.Payments.Constants;
using PaymentOrderAPI.Application.Payments.Strategies;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Application.Payments;

public class PaymentProviderSelector
{
    private const string NoProviderMessage = "No provider supports payment mode '{0}'.";

    private readonly IEnumerable<IProviderFeeStrategy> _strategies;
    private readonly ILogger<PaymentProviderSelector> _logger;

    public PaymentProviderSelector(
        IEnumerable<IProviderFeeStrategy> strategies,
        ILogger<PaymentProviderSelector> logger)
    {
        _strategies = strategies;
        _logger = logger;
    }

    public IProviderFeeStrategy SelectOptimal(PaymentMode mode, decimal totalAmount)
    {
        var candidates = _strategies
            .Where(s => s.Supports(mode))
            .Select(s => new { Strategy = s, Fee = s.CalculateFee(mode, totalAmount) })
            .OrderBy(x => x.Fee)
            .ToList();

        if (candidates.Count == 0)
            throw new NotSupportedException(string.Format(NoProviderMessage, mode));

        var winner = candidates.First();

        _logger.LogInformation(
            LogMessages.ProviderSelected,
            nameof(SelectOptimal), winner.Strategy.ProviderName, winner.Fee, mode, totalAmount);

        return winner.Strategy;
    }
}
