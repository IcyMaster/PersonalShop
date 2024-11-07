using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Products.Dtos;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetProductDetailsWithoutUserAsync(int productId, bool track = true);
        Task<Product?> GetProductDetailsWithUserAsync(int productId, bool track = true);
    }
}