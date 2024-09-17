using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Features.Cart;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<SingleCartDto?> GetCartByUserIdWithProductAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: false);

        if (cart is null)
        {
            return null;
        }

        return new SingleCartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalPrice = cart.TotalPrice,
            CartItems = cart.CartItems.Select(ci => new Domain.Carts.Dtos.CartItemDto
            {
                ProductId = ci.ProductId,
                Quanity = ci.Quanity,
                Product = new Domain.Carts.Dtos.CartItemProductDto
                {
                    Name = ci.Product.Name,
                    Description = ci.Product.Description,
                    Price = ci.Product.Price,
                }
            }).ToList(),
        };
    }
    public async Task<SingleCartDto?> GetCartByCartIdWithOutProductAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByCartIdWithOutProductAsync(cartId, track: false);

        if (cart is null)
        {
            return null;
        }

        return new SingleCartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalPrice = cart.TotalPrice,
            CartItems = cart.CartItems.Select(ci => new Domain.Carts.Dtos.CartItemDto
            {
                ProductId = ci.ProductId,
                Quanity = ci.Quanity,
                Product = null!,
            }).ToList(),
        };
    }
    public async Task<bool> AddCartItemByUserIdAsync(string userId, int productId, int quanity)
    {
        var product = await _productRepository.GetProductByIdWithUserAsync(productId);
        if (product is null)
        {
            return false;
        }

        var item = new CartItem(productId, quanity);

        var cart = await _cartRepository.GetCartByUserIdWithOutProductAsync(userId, track: true);
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

            cart.IncreaseTotalPrice(product.Price * quanity);
        }
        else
        {
            cart = new(userId);
            cart.CartItems.Add(item);
            cart.IncreaseTotalPrice(product.Price * quanity);
            await _cartRepository.AddAsync(cart);
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> DeleteCartItemByUserIdAsync(string userId, int productId)
    {
        var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: true);

        if (cart is null)
        {
            return false;
        }

        var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId == productId);

        if (cartItem is null)
        {
            return false;
        }

        cart.CartItems.Remove(cartItem);

        if(cart.CartItems.Count().Equals(0))
        {
            _cartRepository.Delete(cart);
        }
        else
        {
            cart.SetTotalPrice(0);

            cart.CartItems.ForEach(e => cart.IncreaseTotalPrice(e.Product.Price * e.Quanity));
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> UpdateCartItemQuanityByUserIdAsync(string userId, int productId, int quanity)
    {
        if (quanity < 1)
        {
            return false;
        }

        var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: true);

        if (cart is null)
        {
            return false;
        }

        var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId == productId);

        if (cartItem is null)
        {
            return false;
        }

        if (cartItem.Quanity.Equals(quanity))
        {
            return true;
        }

        cartItem.SetQuantity(quanity);

        cart.SetTotalPrice(0);

        cart.CartItems.ForEach(e => cart.IncreaseTotalPrice(e.Product.Price * e.Quanity));

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return true;
        }

        return false;
    }
}
