using PersonalShop.BusinessLayer.Services.Carts.Dtos;

namespace PersonalShop.BusinessLayer.Common.Interfaces;

public interface ICartQueryRepository
{
    Task<SingleCartDto?> GetCartDetailsWithProductAsync(string userId);
}