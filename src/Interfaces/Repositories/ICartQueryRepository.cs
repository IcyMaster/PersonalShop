using PersonalShop.Features.Carts.Dtos;

namespace PersonalShop.Interfaces.Repositories;

public interface ICartQueryRepository
{
    Task<SingleCartDto?> GetCartDetailsWithProductAsync(string userId);
}