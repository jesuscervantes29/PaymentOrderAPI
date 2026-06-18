using System.Net;
using System.Net.Http.Json;
using PaymentOrderAPI.Application.Orders.DTOs;
using PaymentOrderAPI.IntegrationTests.Fixtures;

namespace PaymentOrderAPI.IntegrationTests;

public class OrdersControllerTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public OrdersControllerTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ValidCashRequest_Returns201WithOrder()
    {
        var request = new CreateOrderRequest("Cash", new List<ProductDto>
        {
            new("Laptop", 1000m)
        });

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
        Assert.NotNull(order);
        Assert.Equal("PagaFacil", order.ProviderName);
        Assert.Equal("Created",   order.Status);
    }

    [Fact]
    public async Task CreateOrder_InvalidPaymentMode_Returns400()
    {
        var request = new CreateOrderRequest("INVALID", new List<ProductDto>
        {
            new("Laptop", 1000m)
        });

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_EmptyProducts_Returns400()
    {
        var request = new CreateOrderRequest("Cash", new List<ProductDto>());

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOrders_ReturnsOkWithList()
    {
        var response = await _client.GetAsync("/api/orders");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOrderById_NotFound_Returns404()
    {
        var response = await _client.GetAsync("/api/orders/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelOrder_NotFound_Returns404()
    {
        var response = await _client.PatchAsync("/api/orders/9999/cancel", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PayOrder_NotFound_Returns404()
    {
        var response = await _client.PatchAsync("/api/orders/9999/pay", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
