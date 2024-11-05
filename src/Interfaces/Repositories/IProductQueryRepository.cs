using PersonalShop.Features.Products.Dtos;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IProductQueryRepository
    {
        Task<List<SingleProductDto>> GetAllProductsWithUserAsync();
        Task<SingleProductDto?> GetProductDetailsWithOutUserAsync(int productId);
        Task<SingleProductDto?> GetProductDetailsWithUserAsync(int productId);
    }
}