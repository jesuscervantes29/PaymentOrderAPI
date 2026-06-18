using System.Net;
using System.Net.Http.Json;
using PaymentOrderAPI.Application.Orders.DTOs;
using PaymentOrderAPI.IntegrationTests.Fixtures;

namespace PaymentOrderAPI.IntegrationTests;

public class ProductsControllerTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ReturnsOkWithProducts()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

        Assert.NotNull(products);
        Assert.NotEmpty(products);
        Assert.All(products, p =>
        {
            Assert.False(string.IsNullOrEmpty(p.Name));
            Assert.True(p.UnitPrice > 0);
        });
    }
}
