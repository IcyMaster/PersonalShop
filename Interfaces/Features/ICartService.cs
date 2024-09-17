using PersonalShop.Domain.Card.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface ICartService
    {
        Task<bool> AddCartItemByUserIdAsync(string userId, int productId, int quanity);
        Task<bool> DeleteCartItemByUserIdAsync(string userId, int productId);
        Task<SingleCartDto?> GetCartByCartIdWithOutProductAsync(Guid cartId);
        Task<SingleCartDto?> GetCartByUserIdWithProductAsync(string userId);
        Task<bool> UpdateCartItemQuanityByUserIdAsync(string userId, int productId, int quanity);
    }
}