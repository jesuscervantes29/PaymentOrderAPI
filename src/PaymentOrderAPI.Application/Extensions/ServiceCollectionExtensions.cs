using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PaymentOrderAPI.Application.Orders;
using PaymentOrderAPI.Application.Payments;
using PaymentOrderAPI.Application.Payments.Strategies;

namespace PaymentOrderAPI.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        services.AddScoped<IProviderFeeStrategy, PagaFacilFeeStrategy>();
        services.AddScoped<IProviderFeeStrategy, CazaPagosFeeStrategy>();
        services.AddScoped<PaymentProviderSelector>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
