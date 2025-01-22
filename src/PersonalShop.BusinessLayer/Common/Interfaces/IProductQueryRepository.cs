using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Common.Interfaces
{
    public interface IProductQueryRepository
    {
        Task<SingleProductDto?> GetProductDetailsWithoutUserAsync(int productId);
        Task<SingleProductDto?> GetProductDetailsWithUserAsync(int productId);
        Task<PagedResult<SingleProductDto>> GetAllProductsWithUserAsync(PagedResultOffset resultOffset);
    }
}