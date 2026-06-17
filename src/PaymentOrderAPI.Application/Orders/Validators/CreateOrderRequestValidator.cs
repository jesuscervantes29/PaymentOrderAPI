using FluentValidation;
using PaymentOrderAPI.Application.Orders.DTOs;
using PaymentOrderAPI.Domain.Enums;

namespace PaymentOrderAPI.Application.Orders.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    private const string InvalidPaymentModeMessage  = "PaymentMode must be Cash, TDC or Transfer.";
    private const string ProductsRequiredMessage    = "At least one product is required.";

    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.PaymentMode)
            .NotEmpty()
            .Must(m => Enum.TryParse<PaymentMode>(m, ignoreCase: true, out _))
            .WithMessage(InvalidPaymentModeMessage);

        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage(ProductsRequiredMessage);

        RuleForEach(x => x.Products).ChildRules(p =>
        {
            p.RuleFor(x => x.Name).NotEmpty();
            p.RuleFor(x => x.UnitPrice).GreaterThan(0);
        });
    }
}
