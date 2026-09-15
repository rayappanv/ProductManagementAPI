using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;

namespace ProductManagement.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<ProductDto> CreateProductAsync(
            CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
        }

        public async Task<bool> UpdateProductAsync(
            int id,
            UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}