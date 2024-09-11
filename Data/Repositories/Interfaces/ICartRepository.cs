using PersonalShop.Domain.Card;

namespace PersonalShop.Data.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserId(string userId);
}
