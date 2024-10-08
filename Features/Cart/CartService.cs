﻿using EasyCaching.Core;
using MassTransit;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Domain.Carts.Commands;
using PersonalShop.Domain.Users;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Features.Cart;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProviderFactory _cachingfactory;

    public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, IEasyCachingProviderFactory cachingfactory)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _cachingfactory = cachingfactory;
    }

    public async Task<SingleCartDto?> GetCartByUserIdWithProductAsync(string userId)
    {
        var cartCashing = _cachingfactory.GetCachingProvider("Carts");

        if (cartCashing.Exists(userId))
        {
            var result = await cartCashing.GetAsync<SingleCartDto>(userId);
            return result.Value;
        }
        else
        {
            var cart = await _cartRepository.GetCartByUserIdWithProductAsync(userId, track: false);

            if (cart is null)
            {
                return null;
            }

            var cartDto = new SingleCartDto
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

            await cartCashing.TrySetAsync(userId, cartDto, TimeSpan.FromHours(1));

            cartDto.ProcessTotalItemCount();

            return cartDto;
        }
    }
    public async Task<SingleCartDto?> GetCartByCartIdWithOutProductAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByCartIdWithOutProductAsync(cartId, track: false);

        if (cart is null)
        {
            return null;
        }

        var cartDto = new SingleCartDto
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

        cartDto.ProcessTotalItemCount();

        return cartDto;
    }
    public async Task<bool> AddCartItemByUserIdAsync(string userId, int productId, int quanity)
    {
        var product = await _productRepository.GetProductByIdWithUserAsync(productId);
        if (product is null)
        {
            return false;
        }

        var item = new CartItem(productId, quanity, product.Price);

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

            cart.ProcessTotalPrice();
        }
        else
        {
            cart = new(userId);
            cart.CartItems.Add(item);
            cart.ProcessTotalPrice();
            await _cartRepository.AddAsync(cart);
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            var cartCashing = _cachingfactory.GetCachingProvider("Carts");
            cartCashing.Remove(userId);

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

        if (cart.CartItems.Count().Equals(0))
        {
            _cartRepository.Delete(cart);
        }
        else
        {
            cart.ProcessTotalPrice();
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            var cartCashing = _cachingfactory.GetCachingProvider("Carts");
            cartCashing.Remove(userId);

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

        cart.ProcessTotalPrice();

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            var cartCashing = _cachingfactory.GetCachingProvider("Carts");
            cartCashing.Remove(userId);

            return true;
        }

        return false;
    }
    public async Task<bool> DeleteProductByProductIdFromAllCartsAsync(DeleteProductFromCartCommand command)
    {
        var cartCashing = _cachingfactory.GetCachingProvider("Carts");

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

                cartCashing.Remove(cart.UserId);
            }
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> UpdateProductInCartsAsync(UpdateProductInCartsCommand command)
    {
        var cartCashing = _cachingfactory.GetCachingProvider("Carts");

        var carts = await _cartRepository.GetAllCartsWithOutProductAsync();

        foreach (var cart in carts)
        {
            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == command.ProductId);

            if (cartItem is not null)
            {
                cartItem.SetItemPrice(command.Price);

                cart.ProcessTotalPrice();

                cartCashing.Remove(cart.UserId);
            }
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return true;
        }

        return false;
    }
}
