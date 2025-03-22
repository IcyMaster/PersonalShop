using EasyCaching.Core;
using PersonalShop.BusinessLayer.Builders.Caches;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Carts.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.Domain.Entities.Carts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Resources.Services.CartService;

namespace PersonalShop.BusinessLayer.Services.Carts;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _cartQueryRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProvider _cachingProvider;

    public CartService(ICartRepository cartRepository,
        ICartQueryRepository cartQueryRepository,
        IProductQueryRepository productQueryRepository,
        IUnitOfWork unitOfWork, IEasyCachingProvider cachingProvider)
    {
        _cartRepository = cartRepository;
        _cartQueryRepository = cartQueryRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _cachingProvider = cachingProvider;
    }

    public async Task<ServiceResult<SingleCartDto>> GetCartDetailsWithCartItemsAsync(string userId)
    {
        string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(userId);

        var cache = await _cachingProvider.GetAsync<SingleCartDto>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<SingleCartDto>.Success(cache.Value);
        }

        var cart = await _cartQueryRepository.GetCartDetailsWithProductAsync(userId);

        if (cart is null)
        {
            return ServiceResult<SingleCartDto>.Success(null!);
        }

        await _cachingProvider.TrySetAsync(userId, cart, TimeSpan.FromHours(1));

        return ServiceResult<SingleCartDto>.Success(cart);
    }
    public async Task<ServiceResult<string>> AddCartItemAsync(string userId, CreateCartItemDto createCartItemDto)
    {
        var product = await _productQueryRepository.GetProductDetailsWithUserAsync(createCartItemDto.ProductId);
        if (product is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        if (product.Stock < createCartItemDto.Quantity)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.ProductStockNotEnough);
        }

        var cartitem = new CartItem(createCartItemDto.ProductId, createCartItemDto.Quantity, product.Price);

        var cart = await _cartRepository.GetCartDetailsWithoutProductAsync(userId);
        if (cart is not null)
        {
            var cartItem = cart.CartItems.FirstOrDefault(e => e.ProductId == createCartItemDto.ProductId);
            if (cartItem is not null)
            {
                cartItem.IncreaseQuantity(createCartItemDto.Quantity);
            }
            else
            {
                cart.CartItems.Add(cartitem);
            }
        }
        else
        {
            cart = new(userId);
            cart.CartItems.Add(cartitem);
            await _cartRepository.AddAsync(cart);
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
    public async Task<ServiceResult<string>> UpdateCartItemQuantityAsync(string userId, int productId, UpdateCartItemDto updateCartItemDto)
    {
        var cart = await _cartRepository.GetCartDetailsWithoutProductAsync(userId, track: true);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var product = await _productQueryRepository.GetProductDetailsWithUserAsync(productId);

        if (product is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        if (product.Stock < updateCartItemDto.Quantity)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.ProductStockNotEnough);
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
    public async Task<ServiceResult<string>> DeleteCartItemAsync(string userId, int productId)
    {
        var cart = await _cartRepository.GetCartDetailsWithoutProductAsync(userId, track: true);

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

    //Consume Events Section
    public async Task<ServiceResult<bool>> DeleteProductFromAllCartsAsync(DeleteProductFromCartCommand command)
    {
        var carts = await _cartRepository.GetAllCartsWithoutProductAsync();

        foreach (var cart in carts)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == command.ProductId);

            if (cartItem is not null)
            {
                cart.CartItems.Remove(cartItem);

                if (cart.CartItems.Count() is 0)
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
    public async Task<ServiceResult<bool>> UpdateProductInAllCartsAsync(UpdateProductInCartsCommand command)
    {
        var carts = await _cartRepository.GetAllCartsWithoutProductAsync();

        foreach (var cart in carts)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == command.ProductId);

            if (cartItem is not null)
            {
                if (cartItem.Quantity > command.Stock)
                {
                    cart.CartItems.Remove(cartItem);
                    if (cart.CartItems.Count() is 0)
                    {
                        _cartRepository.Delete(cart);
                    }
                    else
                    {
                        cart.ProcessTotalPrice();
                    }
                }
                else
                {
                    cartItem.SetItemPrice(command.Price);
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

        return ServiceResult<bool>.Failed(CartServiceErrors.UpdateProductInCartsCommandProblem);
    }
    public async Task<ServiceResult<bool>> UpdateCartItemStockInCartsAsync(UpdateCartItemStockInCartsCommand command)
    {
        var carts = await _cartRepository.GetAllCartsWithoutProductAsync();

        foreach (var cart in carts)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == command.ProductId);

            if (cartItem is not null)
            {
                if (cartItem.Quantity > command.Stock)
                {
                    cart.CartItems.Remove(cartItem);
                    if (cart.CartItems.Count() is 0)
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
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return ServiceResult<bool>.Success(true);
        }

        return ServiceResult<bool>.Failed(CartServiceErrors.UpdateProductInCartsCommandProblem);
    }
}