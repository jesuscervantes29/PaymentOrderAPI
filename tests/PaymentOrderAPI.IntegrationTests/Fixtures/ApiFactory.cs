using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.IntegrationTests.Fixtures;

public class ApiFactory : WebApplicationFactory<Program>
{
    public Mock<IPaymentProviderClient> PagaFacilMock { get; } = new();
    public Mock<IPaymentProviderClient> CazaPagosMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remover los registros reales de IPaymentProviderClient
            var realClients = services
                .Where(d => d.ServiceType == typeof(IPaymentProviderClient))
                .ToList();
            realClients.ForEach(d => services.Remove(d));

            // Configurar mocks
            PagaFacilMock.Setup(c => c.ProviderName).Returns("PagaFacil");
            PagaFacilMock
                .Setup(c => c.CreateOrderAsync(It.IsAny<PaymentMode>(), It.IsAny<IEnumerable<Product>>()))
                .ReturnsAsync(new ProviderOrderResult("EXT-001", 1000m, Enumerable.Empty<Fee>()));
            PagaFacilMock.Setup(c => c.CancelOrderAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            PagaFacilMock.Setup(c => c.PayOrderAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            CazaPagosMock.Setup(c => c.ProviderName).Returns("CazaPagos");
            CazaPagosMock
                .Setup(c => c.CreateOrderAsync(It.IsAny<PaymentMode>(), It.IsAny<IEnumerable<Product>>()))
                .ReturnsAsync(new ProviderOrderResult("EXT-002", 2000m, Enumerable.Empty<Fee>()));
            CazaPagosMock.Setup(c => c.CancelOrderAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            CazaPagosMock.Setup(c => c.PayOrderAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            services.AddTransient<IPaymentProviderClient>(_ => PagaFacilMock.Object);
            services.AddTransient<IPaymentProviderClient>(_ => CazaPagosMock.Object);
        });
    }
}
