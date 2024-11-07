using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Products.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface IProductService
{
    Task<ServiceResult<string>> CreateProductAsync(CreateProductDto createProductDto, string userId);
    Task<ServiceResult<string>> DeleteProductAndValidateOwnerAsync(int productId, string userId);
    Task<ServiceResult<string>> UpdateProductAndValidateOwnerAsync(int productId, UpdateProductDto updateProductDto, string userId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithOutUserAsync(int productId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAndValidateOwnerAsync(int productId, string userId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAsync(int productId);
    Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAsync(PagedResultOffset resultOffset);
    Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(string userId, PagedResultOffset resultOffset);
}