using ProductManagementAPI.DTOs;

namespace ProductManagement.API.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync();

        Task<ProductDto?> GetProductAsync(int id);

        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);

        Task<bool> UpdateProductAsync(
            int id,
            UpdateProductDto updateProductDto);

        Task<bool> DeleteProductAsync(int id);
    }
}