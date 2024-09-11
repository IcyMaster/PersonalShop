using PersonalShop.Data.Repositories.Interfaces;
using PersonalShop.Domain.Card.Dtos;

namespace PersonalShop.Features.Cart;

public class CartService : ICartService
{
    public readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<SingleCartDto?> GetCartByUserIdAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart is null)
        {
            return null;
        }

        return new SingleCartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            CartItems = cart.CartItems,
        };
    }
    //public async Task<Enumerable()>

    //public async Task<bool> AddCartItem(CreateCartItemDto createCartItemDto, string userId)
    //{
    //    var cart = await _cartRepository.GetCartByUserId(userId);
    //    if (cart is null)
    //    {
    //        if (!await _cartRepository.AddCartByUserId(userId))
    //        {
    //            return false;
    //        }
    //    }

    //    if (await _cartRepository.AddCartItem(createCartItemDto, cart.Id))
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
