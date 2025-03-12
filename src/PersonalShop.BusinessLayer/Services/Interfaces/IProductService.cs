using PersonalShop.BusinessLayer.Services.Products.Commands;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Services.Interfaces;

public interface IProductService
{
    Task<ServiceResult<string>> CreateProductAsync(CreateProductDto createProductDto, string userId);
    Task<ServiceResult<string>> DeleteProductAndValidateOwnerAsync(int productId, string userId);
    Task<ServiceResult<string>> UpdateProductAndValidateOwnerAsync(int productId, UpdateProductDto updateProductDto, string userId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithOutUserAsync(int productId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAndValidateOwnerAsync(int productId, string userId);
    Task<ServiceResult<SingleProductDto>> GetProductDetailsWithUserAsync(int productId);
    Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAsync(PagedResultOffset resultOffset);
    Task<ServiceResult<PagedResult<SingleProductDto>>> GetAllProductsWithUserAndValidateOwnerAsync(PagedResultOffset resultOffset, string userId);
    Task<ServiceResult<bool>> UpdateProductStockAsync(UpdateProductStockCommand command);
}