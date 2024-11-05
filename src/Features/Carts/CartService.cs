using EasyCaching.Core;
using PersonalShop.Builders.Caches;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Carts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Carts.Commands;
using PersonalShop.Features.Carts.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Resources.Services.CartService;

namespace PersonalShop.Features.Carts;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProvider _cachingProvider;

    public CartService(ICartRepository cartRepository, IProductQueryRepository productQueryRepository,
        IUnitOfWork unitOfWork,IEasyCachingProvider cachingProvider)
    {
        _cartRepository = cartRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _cachingProvider = cachingProvider;
    }

    public async Task<ServiceResult<SingleCartDto>> GetCartByUserIdWithProductAsync(string userId)
    {
        string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(userId);

        var cache = await _cachingProvider.GetAsync<SingleCartDto>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<SingleCartDto>.Success(cache.Value);
        }

        var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: false);

        if (cart is null)
        {
            return ServiceResult<SingleCartDto>.Failed(CartServiceErrors.CartNotFound);
        }

        var cartDto = new SingleCartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalPrice = cart.TotalPrice,
            CartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Product = new CartItemProductDto
                {
                    Name = ci.Product.Name,
                    Description = ci.Product.Description,
                    Price = ci.Product.Price,
                }
            }).ToList(),
        };

        await _cachingProvider.TrySetAsync(userId, cartDto, TimeSpan.FromHours(1));

        cartDto.ProcessTotalItemCount();

        return ServiceResult<SingleCartDto>.Success(cartDto);
    }
    public async Task<ServiceResult<SingleCartDto>> GetCartByCartIdWithOutProductAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByCartIdWithOutProductAsync(cartId, track: false);

        if (cart is null)
        {
            return ServiceResult<SingleCartDto>.Failed(CartServiceErrors.CartNotFound);
        }

        var cartDto = new SingleCartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalPrice = cart.TotalPrice,
            CartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Product = null!,
            }).ToList(),
        };

        cartDto.ProcessTotalItemCount();

        return ServiceResult<SingleCartDto>.Success(cartDto);
    }
    public async Task<ServiceResult<string>> AddCartItemByUserIdAsync(string userId, CreateCartItemDto createCartItemDto)
    {
        var product = await _productQueryRepository.GetProductDetailsWithUserAsync(createCartItemDto.ProductId);
        if (product is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var item = new CartItem(createCartItemDto.ProductId, createCartItemDto.Quantity, product.Price);

        var cart = await _cartRepository.GetCartByUserIdWithOutProductAsync(userId, track: true);
        if (cart is not null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId.Equals(createCartItemDto.ProductId));
            if (cartItem is not null)
            {
                cartItem.IncreaseQuantity(createCartItemDto.Quantity);
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

        cart.ProcessTotalPrice();

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(userId);

            await _cachingProvider.RemoveAsync(cacheKey);

            return ServiceResult<string>.Success(CartServiceSuccess.SuccessfulAddCartItem);
        }

        return ServiceResult<string>.Failed(CartServiceErrors.AddCartItemProblem);
    }
    public async Task<ServiceResult<string>> DeleteCartItemByUserIdAsync(string userId, int productId)
    {
        var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: true);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId == productId);

        if (cartItem is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartItemNotFound);
        }

        cart.CartItems.Remove(cartItem);

        if (cart.CartItems.Count() is 0)
        {
            _cartRepository.Delete(cart);
        }
        else
        {
            cart.ProcessTotalPrice();
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(userId);

            await _cachingProvider.RemoveAsync(cacheKey);

            return ServiceResult<string>.Success(CartServiceSuccess.SuccessfulDeleteCartItem);
        }

        return ServiceResult<string>.Failed(CartServiceErrors.DeleteCartItemProblem);
    }
    public async Task<ServiceResult<string>> UpdateCartItemQuantityByUserIdAsync(string userId, int productId, UpdateCartItemDto updateCartItemDto)
    {
        var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: true);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId == productId);

        if (cartItem is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartItemNotFound);
        }

        if (cartItem.Quantity == updateCartItemDto.Quantity)
        {
            return ServiceResult<string>.Success(CartServiceSuccess.SuccessfulUpdateCartItem);
        }

        cartItem.SetQuantity(updateCartItemDto.Quantity);

        cart.ProcessTotalPrice();

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(userId);

            await _cachingProvider.RemoveAsync(cacheKey);

            return ServiceResult<string>.Success(CartServiceSuccess.SuccessfulUpdateCartItem);
        }

        return ServiceResult<string>.Failed(CartServiceSuccess.SuccessfulUpdateCartItem);
    }
    public async Task<ServiceResult<bool>> DeleteProductByProductIdFromAllCartsAsync(DeleteProductFromCartCommand command)
    {
        var carts = await _cartRepository.GetAllCartsWithOutProductAsync();

        foreach (var cart in carts)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == command.ProductId);

            if (cartItem is not null)
            {
                cart.CartItems.Remove(cartItem);

                if (cart.CartItems.Count() == 0)
                {
                    _cartRepository.Delete(cart);
                }
                else
                {
                    cart.ProcessTotalPrice();
                }

                string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(cart.UserId);

                await _cachingProvider.RemoveAsync(cacheKey);
            }
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return ServiceResult<bool>.Success(true);
        }

        return ServiceResult<bool>.Failed(CartServiceErrors.DeleteProductInCartsCommandProblem);
    }
    public async Task<ServiceResult<bool>> UpdateProductInCartsAsync(UpdateProductInCartsCommand command)
    {
        var carts = await _cartRepository.GetAllCartsWithOutProductAsync();

        foreach (var cart in carts)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == command.ProductId);

            if (cartItem is not null)
            {
                cartItem.SetItemPrice(command.Price);

                cart.ProcessTotalPrice();

                string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(cart.UserId);

                await _cachingProvider.RemoveAsync(cacheKey);
            }
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return ServiceResult<bool>.Success(true);
        }

        return ServiceResult<bool>.Failed(CartServiceErrors.UpdateProductInCartsCommandProblem);
    }
}
