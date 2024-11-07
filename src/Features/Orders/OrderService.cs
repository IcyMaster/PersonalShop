using EasyCaching.Core;
using PersonalShop.Builders.Caches;
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
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProvider _cachingProvider;

    public OrderService(IOrderRepository orderRepository,IOrderQueryRepository orderQueryRepository,
        IProductQueryRepository productQueryRepository,ICartRepository cartRepository, IUnitOfWork unitOfWork,
        IEasyCachingProvider cachingProvider)
    {
        _orderRepository = orderRepository;
        _orderQueryRepository = orderQueryRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
        _cachingProvider = cachingProvider;
    }

    public async Task<ServiceResult<string>> CreateOrderByUserIdAsync(string userId)
    {
        var cart = await _cartRepository.GetCartDetailsWithoutProductAsync(userId, track: false);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var order = new Order(cart.UserId, cart.TotalPrice);

        foreach (var item in cart.CartItems)
        {
            var product = await _productQueryRepository.GetProductDetailsWithoutUserAsync(item.ProductId);
            if (product is not null)
            {
                order.OrderItems.Add(new OrderItem(item.ProductId, product.Name, product.Price, item.Quantity));
            }
        }

        await _orderRepository.AddAsync(order);

        _cartRepository.Delete(cart);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            string cacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(cart.UserId);

            await _cachingProvider.RemoveAsync(cacheKey);

            return ServiceResult<string>.Success(OrderServiceSuccess.SuccessfulCreateOrder);
        }

        return ServiceResult<string>.Failed(OrderServiceErrors.CreateOrderProblem);
    }
    public async Task<ServiceResult<List<SingleOrderDto>>> GetAllOrderByUserIdAsync(string userId)
    {
        var listOfOrders = await _orderQueryRepository.GetAllOrdersAsync(userId);

        return ServiceResult<List<SingleOrderDto>>.Success(listOfOrders);
    }
}
