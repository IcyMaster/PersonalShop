using PersonalShop.Data.Repositories.Interfaces;
using PersonalShop.Domain.Card.Dtos;

namespace PersonalShop.Features.Card;

public class CartService
{
    public readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<SingleCartDto?> GetCartByUserId(string userId)
    {
        return await _cartRepository.GetCartByUserId(userId);
    }
    public async Task<bool> AddCartItem(CreateCartItemDto createCartItemDto, string userId)
    {
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart is null)
        {
            if (!await _cartRepository.AddCartByUserId(userId))
            {
                return false;
            }
        }

        if (await _cartRepository.AddCartItem(createCartItemDto, cart.Id))
        {
            return true;
        }

        return false;
    }
}
