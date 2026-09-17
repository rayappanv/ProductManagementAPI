using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;
using ProductManagementAPI.Repositories;

namespace ProductManagementAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            })
                .ToList();
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity
            };

            await _productRepository.AddAsync(product);

            await _productRepository.SaveChangesAsync();
            _logger.LogInformation("Product created successfully. ProductId: {ProductId}, Name: {ProductName}", product.Id, product.Name);

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
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product update failed because product was not found. ProductId: {ProductId}", id);

                return false;
            }

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;

            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product delete failed because product was not found. ProductId: {ProductId}", id);

                return false;
            }

            _productRepository.Delete(product);

            await _productRepository.SaveChangesAsync();
            _logger.LogInformation("Product deleted successfully. ProductId: {ProductId}", id);

            return true;
        }
    }
}