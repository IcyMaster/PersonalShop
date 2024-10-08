﻿using EasyCaching.Core;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Orders.Dtos;
using PersonalShop.Domain.Users;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Features.Order;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProviderFactory _cachingfactory;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ICartRepository cartRepository, IUnitOfWork unitOfWork, IEasyCachingProviderFactory cachingfactory)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
        _cachingfactory = cachingfactory;
    }

    public async Task<bool> CreateOrderByCartIdAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByCartIdWithOutProductAsync(cartId, track: false);

        if (cart is null)
        {
            return false;
        }

        var order = new Domain.Orders.Order(cart.UserId, cart.TotalPrice);

        cart.CartItems.ForEach(async e =>
        {
            var product = await _productRepository.GetProductByIdWithOutUserAsync(e.ProductId,track: false);
            if(product is not null)
            {
                order.OrderItems.Add(new OrderItem(e.ProductId,product.Name,product.Price, e.Quanity));
            }
        });

        await _orderRepository.AddAsync(order);

        _cartRepository.Delete(cart);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            var cartCashing = _cachingfactory.GetCachingProvider("Carts");
            cartCashing.Remove(cart.UserId);

            return true;
        }

        return false;
    }
    public async Task<List<SingleOrderDto>> GetAllOrderByUserIdAsync(string userId)
    {
        return await _orderRepository.GetAllOrdersByUserIdAsync(userId);
    }
}
