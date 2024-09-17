using PersonalShop.Domain.Products.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface IProductService
{
    Task<bool> AddProductByUserIdAsync(CreateProductDto createProductDto, string userId);
    Task<bool> DeleteProductByIdAndValidateOwnerAsync(int id, string userId);
    Task<List<SingleProductDto>> GetAllProductsWithUserAndValidateOwnerAsync(string userId);
    Task<List<SingleProductDto>> GetAllProductsWithUserAsync();
    Task<SingleProductDto?> GetProductByIdWithUserAndValidateOwnerAsync(int id, string userId);
    Task<SingleProductDto?> GetProductByIdWithUserAsync(int id);
    Task<bool> UpdateProductByIdAndValidateOwnerAsync(int id, UpdateProductDto updateProductDto, string userId);
}