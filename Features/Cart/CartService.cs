using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Features.Cart;

public class CartService : ICartService
{
    public readonly ICartRepository _cartRepository;
    public readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;

    public CartService(ICartRepository cartRepository, IProductService productService, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productService = productService;
        _unitOfWork = unitOfWork;
    }

    public async Task<SingleCartDto?> GetCartByUserIdAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId, track: false);

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
    public async Task<bool> AddCartItemByUserIdAsync(string userId, long productId, int quanity)
    {
        var product = await _productService.GetProductById(productId);
        if (product is null)
        {
            return false;
        }

        var item = new CartItem(productId, quanity);

        var cart = await _cartRepository.GetCartByUserIdAsync(userId, track: true);
        if (cart is not null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId.Equals(productId));
            if (cartItem is not null)
            {
                cartItem.IncreaseQuantity(quanity);
            }
            else
            {
                cart.CartItems.Add(item);
            }
        }
        else
        {
            cart = new(userId);
            cart.CartItems.Add(item);
        }

        await _unitOfWork.SaveChangesAsync(true);

        return true;
    }
}
