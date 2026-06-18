using FluentValidation;
using Microsoft.Extensions.Logging;
using PaymentOrderAPI.Application.Common.Exceptions;
using PaymentOrderAPI.Application.Orders.Constants;
using PaymentOrderAPI.Application.Orders.DTOs;
using PaymentOrderAPI.Application.Payments;
using PaymentOrderAPI.Domain.Entities;
using PaymentOrderAPI.Domain.Enums;
using PaymentOrderAPI.Domain.Interfaces;

namespace PaymentOrderAPI.Application.Orders;

public class OrderService : IOrderService
{
    private readonly PaymentProviderSelector _selector;
    private readonly IEnumerable<IPaymentProviderClient> _clients;
    private readonly IOrderRepository _repository;
    private readonly IValidator<CreateOrderRequest> _createValidator;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        PaymentProviderSelector selector,
        IEnumerable<IPaymentProviderClient> clients,
        IOrderRepository repository,
        IValidator<CreateOrderRequest> createValidator,
        ILogger<OrderService> logger)
    {
        _selector        = selector;
        _clients         = clients;
        _repository      = repository;
        _createValidator = createValidator;
        _logger          = logger;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var mode = Enum.Parse<PaymentMode>(request.PaymentMode, ignoreCase: true);

        var products = request.Products
            .Select(p => new Product { Name = p.Name, UnitPrice = p.UnitPrice })
            .ToList();

        var total    = products.Sum(p => p.UnitPrice);
        var strategy = _selector.SelectOptimal(mode, total);
        var client   = _clients.First(c => c.ProviderName == strategy.ProviderName);
        var result   = await client.CreateOrderAsync(mode, products);

        var order = new Order
        {
            ExternalOrderId = result.ExternalOrderId,
            ProviderName    = strategy.ProviderName,
            Status          = OrderStatus.Created,
            PaymentMode     = mode,
            Products        = products,
            Fees            = result.Fees.ToList(),
            Amount          = total
        };

        var saved = await _repository.CreateAsync(order);

        _logger.LogInformation(
            LogMessages.OrderCreated,
            nameof(CreateOrderAsync), saved.Id, saved.ProviderName, saved.Amount);

        return MapToResponse(saved);
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersAsync()
    {
        var orders = await _repository.GetAllAsync();
        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponse> GetOrderByIdAsync(int orderId)
    {
        var order = await _repository.GetByIdAsync(orderId)
            ?? throw new NotFoundException(nameof(Order), orderId);

        return MapToResponse(order);
    }

    public async Task CancelOrderAsync(int orderId)
    {
        var order = await _repository.GetByIdAsync(orderId)
            ?? throw new NotFoundException(nameof(Order), orderId);

        var client = _clients.First(c => c.ProviderName == order.ProviderName);
        await client.CancelOrderAsync(order.ExternalOrderId);
        await _repository.UpdateStatusAsync(orderId, OrderStatus.Cancelled);

        _logger.LogInformation(
            LogMessages.OrderCancelled,
            nameof(CancelOrderAsync), orderId, order.ProviderName);
    }

    public async Task PayOrderAsync(int orderId)
    {
        var order = await _repository.GetByIdAsync(orderId)
            ?? throw new NotFoundException(nameof(Order), orderId);

        var client = _clients.First(c => c.ProviderName == order.ProviderName);
        await client.PayOrderAsync(order.ExternalOrderId);
        await _repository.UpdateStatusAsync(orderId, OrderStatus.Paid);

        _logger.LogInformation(
            LogMessages.OrderPaid,
            nameof(PayOrderAsync), orderId, order.ProviderName);
    }

    private static OrderResponse MapToResponse(Order order) => new(
        order.Id,
        order.ExternalOrderId,
        order.ProviderName,
        order.Status.ToString(),
        order.PaymentMode.ToString(),
        order.Products.Select(p => new ProductDto(p.Name, p.UnitPrice)).ToList(),
        order.Fees.Select(f => new FeeDto(f.Name, f.Amount)).ToList(),
        order.Amount,
        order.CreatedAt
    );
}
