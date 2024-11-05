using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllProductsWithUserAsync(int productId, bool track = true);
        Task<Product?> GetProductDetailsWithOutUserAsync(int productId, bool track = true);
        Task<Product?> GetProductDetailsWithUserAsync(int productId, bool track = true);
    }
}