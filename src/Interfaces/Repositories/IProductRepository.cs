using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllProductsWithUserAsync(bool track = true);
        Task<Product?> GetProductByIdWithOutUserAsync(int id, bool track = true);
        Task<Product?> GetProductByIdWithUserAsync(int id, bool track = true);
    }
}