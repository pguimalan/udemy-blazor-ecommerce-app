using BlazorEcommerceApp.Server.Services.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var response = await _productService.GetProductsAsync();
            return Ok(response);
        }

        [HttpGet("{productId}")]        
        public async Task<ActionResult<Product>> GetProduct(int productId)
        {
            var response = await _productService.GetProductAsync(productId);
            return Ok(response);
        }
    }
}
