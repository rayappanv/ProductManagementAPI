using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> products =
       [
           new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 65000,
                StockQuantity = 10
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Price = 1200,
                StockQuantity = 25
            },
            new Product
            {
                Id = 3,
                Name = "Keyboard",
                Price = 2500,
                StockQuantity = 15
            }
       ];

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} was not found");

            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            product.Id = products.Count == 0 ? 1 : products.Max(p => p.Id) + 1;

            products.Add(product);
           
       
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} was not found");
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.StockQuantity = updatedProduct.StockQuantity;

            return Ok(existingProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} was not found");
            }

            products.Remove(product);
            return Ok(product);
        }
    }
}