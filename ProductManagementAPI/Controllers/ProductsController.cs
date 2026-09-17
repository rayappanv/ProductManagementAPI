using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Services;
using ProductManagementAPI.DTOs;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =
                await _productService.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product =
                await _productService.GetProductAsync(id);

            if (product == null)
            {
                return NotFound(
                    $"Product with ID {id} was not found");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            CreateProductDto createProductDto)
        {
            var product =
                await _productService.CreateProductAsync(createProductDto);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            UpdateProductDto updateProductDto)
        {
            var updated =
                await _productService.UpdateProductAsync(
                    id,
                    updateProductDto);

            if (!updated)
            {
                return NotFound(
                    $"Product with ID {id} was not found");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted =
                await _productService.DeleteProductAsync(id);

            if (!deleted)
            {
                return NotFound(
                    $"Product with ID {id} was not found");
            }

            return NoContent();
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception");
        }
    }
}