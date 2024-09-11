using PersonalShop.Domain.Card.Dtos;

namespace PersonalShop.Features.Cart
{
    public interface ICartService
    {
        Task<SingleCartDto?> GetCartByUserIdAsync(string userId);
    }
}