using PersonalShop.Domain.Card;

namespace PersonalShop.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByCartIdWithOutProductAsync(Guid cartId, bool track);
        Task<Cart?> GetCartByUserIdWithOutProductAsync(string userId, bool track);
        Task<Cart?> GetCartByUserIdWithProductAsync(string userId, bool track);
    }
}