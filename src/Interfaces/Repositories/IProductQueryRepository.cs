using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Products.Dtos;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IProductQueryRepository
    {
        Task<SingleProductDto?> GetProductDetailsWithoutUserAsync(int productId);
        Task<SingleProductDto?> GetProductDetailsWithUserAsync(int productId);
        Task<PagedResult<SingleProductDto>> GetAllProductsWithUserAsync(PagedResultOffset resultOffset);
    }
}