using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Domain.Response;

namespace PersonalShop.Interfaces.Features;
public interface IProductService
{
    Task<ServiceResult<string>> CreateProductByUserIdAsync(CreateProductDto createProductDto, string userId);
    Task<ServiceResult<string>> DeleteProductByIdAndValidateOwnerAsync(int id, string userId);
    Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(string userId);
    Task<ServiceResult<List<SingleProductDto>>> GetAllProductsWithUserAsync();
    Task<ServiceResult<SingleProductDto>> GetProductByIdWithOutUserAsync(int id);
    Task<ServiceResult<SingleProductDto>> GetProductByIdWithUserAndValidateOwnerAsync(int id, string userId);
    Task<ServiceResult<SingleProductDto>> GetProductByIdWithUserAsync(int id);
    Task<ServiceResult<string>> UpdateProductByIdAndValidateOwnerAsync(int id, UpdateProductDto updateProductDto, string userId);
}