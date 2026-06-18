using Microsoft.AspNetCore.Mvc;
using PaymentOrderAPI.API.DTOs;
using PaymentOrderAPI.Application.Products;
using PaymentOrderAPI.Application.Products.DTOs;

namespace PaymentOrderAPI.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductCatalogDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _productService.GetProductsAsync();
        return Ok(result);
    }
}
