using PersonalShop.Domain.Responses;
using PersonalShop.Features.Products.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface IProductService
{
    Task<ServiceResult<string>> CreateProductAsync(CreateProductDto createProductDto, string userId);
    Task<ServiceResult<string>> DeleteProductAndValidateOwnerAsync(int productId, string userId);
    Task<ServiceResult<string>> UpdateProductAndValidateOwnerAsync(int productId, UpdateProductDto updateProductDto, string userId);
    Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAsync();
    Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(string userId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithOutUserAsync(int productId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAndValidateOwnerAsync(int productId, string userId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAsync(int productId);
}