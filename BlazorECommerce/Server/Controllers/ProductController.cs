using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerce.Server.Controllers
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
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProduct(int productId)
        {
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var result = await _productService.GetProductsByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ProductSearchResult>> SearchProducts(string searchText, int page = 1)
        {
            var result = await _productService.SearchProducts(searchText, page);
            return Ok(result);
        }

        [HttpGet("searchsuggestion/{searchText}")]
        public async Task<ActionResult<List<Product>>> GetProductSearchSuggestion(string searchText)
        {
            var result = await _productService.GetProductSearchSuggestion(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<Product>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProducts();
            return Ok(result);
        }
    }
}
