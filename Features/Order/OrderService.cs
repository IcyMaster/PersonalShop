﻿using Microsoft.EntityFrameworkCore;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Orders.Dtos;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Features.Order;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
    }

    public async Task<bool> CreateOrderByCartIdAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetCartByCartIdWithOutProductAsync(cartId, track: false);

        if (cart is null)
        {
            return false;
        }

        var order = new Domain.Orders.Order(cart.UserId, cart.TotalPrice);

        cart.CartItems.ForEach(e =>
        {
            order.OrderItems.Add(new OrderItem(e.ProductId, e.Quanity));
        });

        await _orderRepository.AddAsync(order);

        _cartRepository.Delete(cart);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            return true;
        }

        return false;
    }
    public async Task<List<SingleOrderDto>?> GetAllOrderByUserId(string userId)
    {
        var data = await _orderRepository.GetOrderslistByUserIdAsync(userId);
        if (data is null)
        {
            return null;
        }

        return data.Select(ob => new SingleOrderDto
        {
            Id = ob.Id,
            UserId = ob.UserId,
            User = new OrderUserDto
            {
                FirstName = ob.User.FirstName,
                LastName = ob.User.LastName,
                Email = ob.User.Email!,
            },

            TotalPrice = ob.TotalPrice,
            OrderDate = ob.OrderDate,
            OrderItems = ob.OrderItems.Select(oi => new SingleOrderItemDto
            {
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                Quanity = oi.Quanity,

                Product = new OrderProductDto
                {
                    Name = oi.Product.Name,
                    Description = oi.Product.Description,
                    Price = oi.Product.Price,
                }

            }).ToList()

        }).ToList();
    }
}