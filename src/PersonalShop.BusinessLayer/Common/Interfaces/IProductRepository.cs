using PersonalShop.Domain.Entities.Products;

namespace PersonalShop.BusinessLayer.Common.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetProductDetailsWithoutUserAsync(int productId, bool track = true);
        Task<Product?> GetProductDetailsWithUserAsync(int productId, bool track = true);
    }
}