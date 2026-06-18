using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentOrderAPI.Domain.Interfaces;
using PaymentOrderAPI.Infrastructure.Configuration;
using PaymentOrderAPI.Infrastructure.Providers;
using PaymentOrderAPI.Infrastructure.Repositories;

namespace PaymentOrderAPI.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        services.Configure<ProviderSettings>(configuration.GetSection("Providers"));

        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();

        services.AddHttpClient<PagaFacilClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ProviderSettings>>().Value;
            client.BaseAddress = new Uri(settings.PagaFacil.BaseUrl);
            client.DefaultRequestHeaders.Add("x-api-key", settings.PagaFacil.ApiKey);
        });

        services.AddHttpClient<CazaPagosClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ProviderSettings>>().Value;
            client.BaseAddress = new Uri(settings.CazaPagos.BaseUrl);
            client.DefaultRequestHeaders.Add("x-api-key", settings.CazaPagos.ApiKey);
        });

        // Registrar ambos como IPaymentProviderClient para inyectar IEnumerable<IPaymentProviderClient>
        services.AddTransient<IPaymentProviderClient>(sp => sp.GetRequiredService<PagaFacilClient>());
        services.AddTransient<IPaymentProviderClient>(sp => sp.GetRequiredService<CazaPagosClient>());

        return services;
    }
}
