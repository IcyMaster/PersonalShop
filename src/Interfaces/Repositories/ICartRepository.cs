using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card;

namespace PersonalShop.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<List<Cart>> GetAllCartsWithOutProductAsync();
        Task<Cart?> GetCartByCartIdWithOutProductAsync(Guid cartId, bool track = true);
        Task<Cart?> GetCartByUserIdWithOutProductAsync(string userId, bool track = true);
        Task<Cart?> GetCartByUserIdWithProductAsync(string userId, bool track = true);
    }
}