using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card;

namespace PersonalShop.Interfaces.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(string userId, bool track);
}
