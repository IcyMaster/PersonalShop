﻿using EasyCaching.Core;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Orders.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Resources.Services.CartService;
using PersonalShop.Resources.Services.OrderService;

namespace PersonalShop.Features.Orders;

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

    public async Task<ServiceResult<string>> CreateOrderByCartIdAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByCartIdWithOutProductAsync(cartId, track: false);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var order = new Order(cart.UserId, cart.TotalPrice);

        cart.CartItems.ForEach(async e =>
        {
            var product = await _productRepository.GetProductByIdWithOutUserAsync(e.ProductId, track: false);
            if (product is not null)
            {
                order.OrderItems.Add(new OrderItem(e.ProductId, product.Name, product.Price, e.Quantity));
            }
        });

        await _orderRepository.AddAsync(order);

        _cartRepository.Delete(cart);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            var cartCashing = _cachingfactory.GetCachingProvider("Carts");
            cartCashing.Remove(cart.UserId);

            return ServiceResult<string>.Success(OrderServiceSuccess.SuccessfulCreateOrder);
        }

        return ServiceResult<string>.Failed(OrderServiceErrors.CreateOrderProblem);
    }
    public async Task<ServiceResult<string>> CreateOrderByUserIdAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdWithOutProductAsync(userId, track: false);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var order = new Order(cart.UserId, cart.TotalPrice);

        foreach (var item in cart.CartItems)
        {
            var product = await _productRepository.GetProductByIdWithOutUserAsync(item.ProductId, track: false);
            if (product is not null)
            {
                order.OrderItems.Add(new OrderItem(item.ProductId, product.Name, product.Price, item.Quantity));
            }
        }

        await _orderRepository.AddAsync(order);

        _cartRepository.Delete(cart);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            var cartCashing = _cachingfactory.GetCachingProvider("Carts");
            cartCashing.Remove(cart.UserId);

            return ServiceResult<string>.Success(OrderServiceSuccess.SuccessfulCreateOrder);
        }

        return ServiceResult<string>.Failed(OrderServiceErrors.CreateOrderProblem);
    }
    public async Task<ServiceResult<List<SingleOrderDto>>> GetAllOrderByUserIdAsync(string userId)
    {
        var listOfOrders = await _orderRepository.GetAllOrdersByUserIdAsync(userId);
        return ServiceResult<List<SingleOrderDto>>.Success(listOfOrders);
    }
}