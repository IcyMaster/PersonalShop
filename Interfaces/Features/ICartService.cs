using PersonalShop.Domain.Card.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface ICartService
    {
        Task<bool> AddCartItemByUserIdAsync(string userId, long productId, int quanity);
        Task<SingleCartDto?> GetCartByUserIdAsync(string userId);
    }
}