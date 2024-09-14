using PersonalShop.Domain.Card.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface ICartService
    {
        Task<bool> AddCartItemByUserIdAsync(string userId, long productId, int quanity);
        Task<bool> DeleteCartItemByUserIdAsync(string userId, long productId);
        Task<SingleCartDto?> GetCartByCartIdAsync(Guid cartId);
        Task<SingleCartDto?> GetCartByUserIdAsync(string userId);
        Task<bool> UpdateCartItemQuanityByUserIdAsync(string userId, long productId, int quanity);
    }
}