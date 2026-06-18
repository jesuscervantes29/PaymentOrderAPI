using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentOrderAPI.Application.Orders;
using PaymentOrderAPI.Application.Orders.DTOs;
using PaymentOrderAPI.Application.Common.Exceptions;
using PaymentOrderAPI.Application.Payments;
using PaymentOrderAPI.Application.Payments.Strategies;
using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.UnitTests.Orders;

public class OrderServiceTests
{
    private readonly Mock<IPaymentProviderClient> _pagaFacilMock;
    private readonly Mock<IOrderRepository>       _repositoryMock;
    private readonly Mock<IValidator<CreateOrderRequest>> _validatorMock;
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _pagaFacilMock  = new Mock<IPaymentProviderClient>();
        _repositoryMock = new Mock<IOrderRepository>();
        _validatorMock  = new Mock<IValidator<CreateOrderRequest>>();

        _pagaFacilMock.Setup(c => c.ProviderName).Returns("PagaFacil");

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateOrderRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var strategies = new IProviderFeeStrategy[]
        {
            new PagaFacilFeeStrategy(),
            new CazaPagosFeeStrategy()
        };

        var selector = new PaymentProviderSelector(strategies, NullLogger<PaymentProviderSelector>.Instance);

        _sut = new OrderService(
            selector,
            new[] { _pagaFacilMock.Object },
            _repositoryMock.Object,
            _validatorMock.Object,
            NullLogger<OrderService>.Instance);
    }

    [Fact]
    public async Task CreateOrderAsync_ValidCashRequest_ReturnsOrderResponse()
    {
        var request = new CreateOrderRequest("Cash", new List<ProductDto>
        {
            new("Laptop", 1000m)
        });

        _pagaFacilMock
            .Setup(c => c.CreateOrderAsync(PaymentMode.Cash, It.IsAny<IEnumerable<Product>>()))
            .ReturnsAsync(new ProviderOrderResult("EXT-001", 1000m, Enumerable.Empty<Fee>()));

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => { o.Id = 1; return o; });

        var result = await _sut.CreateOrderAsync(request);

        Assert.Equal(1,           result.Id);
        Assert.Equal("EXT-001",   result.ExternalOrderId);
        Assert.Equal("PagaFacil", result.ProviderName);
        Assert.Equal("Created",   result.Status);
    }

    [Fact]
    public async Task GetOrderByIdAsync_OrderNotFound_ThrowsNotFoundException()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.GetOrderByIdAsync(99));
    }

    [Fact]
    public async Task CancelOrderAsync_OrderNotFound_ThrowsNotFoundException()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.CancelOrderAsync(99));
    }

    [Fact]
    public async Task PayOrderAsync_OrderNotFound_ThrowsNotFoundException()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.PayOrderAsync(99));
    }

    [Fact]
    public async Task CancelOrderAsync_ValidOrder_CallsProviderAndUpdatesStatus()
    {
        var order = new Order { Id = 1, ExternalOrderId = "EXT-001", ProviderName = "PagaFacil" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
        _repositoryMock.Setup(r => r.UpdateStatusAsync(1, OrderStatus.Cancelled)).ReturnsAsync(order);
        _pagaFacilMock.Setup(c => c.CancelOrderAsync("EXT-001")).Returns(Task.CompletedTask);

        await _sut.CancelOrderAsync(1);

        _pagaFacilMock.Verify(c => c.CancelOrderAsync("EXT-001"), Times.Once);
        _repositoryMock.Verify(r => r.UpdateStatusAsync(1, OrderStatus.Cancelled), Times.Once);
    }
}
