using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Carts;

namespace PersonalShop.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<List<Cart>> GetAllCartsWithoutProductAsync(bool track = true);
        Task<Cart?> GetCartDetailsWithoutProductAsync(string userId, bool track = true);
    }
}