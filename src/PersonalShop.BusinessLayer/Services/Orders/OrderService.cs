using EasyCaching.Core;
using MassTransit;
using PersonalShop.BusinessLayer.Builders.Caches;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Carts.Consumers;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.BusinessLayer.Services.Products.Commands;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Orders;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Contracts;
using PersonalShop.Shared.Resources.Services.CartService;
using PersonalShop.Shared.Resources.Services.OrderService;

namespace PersonalShop.BusinessLayer.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IBus _bus;

    public OrderService(IOrderRepository orderRepository,
        IOrderQueryRepository orderQueryRepository,
        IProductQueryRepository productQueryRepository,
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IEasyCachingProvider cachingProvider,
        IBus bus)
    {
        _orderRepository = orderRepository;
        _orderQueryRepository = orderQueryRepository;
        _productQueryRepository = productQueryRepository;
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
        _cachingProvider = cachingProvider;
        _bus = bus;
    }

    public async Task<ServiceResult<string>> CreateOrderAsync(string userId)
    {
        var cart = await _cartRepository.GetCartDetailsWithoutProductAsync(userId, track: false);

        if (cart is null)
        {
            return ServiceResult<string>.Failed(CartServiceErrors.CartNotFound);
        }

        var order = new Order(cart.UserId, cart.TotalPrice, OrderStatus.NoStatus);

        var updateStockEvents = new List<UpdateProductStockCommand>();
        var updateCartItemStockEvents = new List<UpdateCartItemStockInCartsCommand>();

        foreach (var item in cart.CartItems)
        {
            var product = await _productQueryRepository.GetProductDetailsWithoutUserAsync(item.ProductId);
            if (product is not null)
            {
                order.OrderItems.Add(new OrderItem(item.ProductId, product.Name, product.Price, item.Quantity));

                updateStockEvents.Add(new UpdateProductStockCommand
                {
                    ProductId = item.ProductId,
                    Stock = product.Stock - item.Quantity,
                });

                updateCartItemStockEvents.Add(new UpdateCartItemStockInCartsCommand
                {
                    ProductId = item.ProductId,
                    Stock = product.Stock - item.Quantity,
                });
            }
        }

        await _orderRepository.AddAsync(order);

        _cartRepository.Delete(cart);

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            string cardCacheKey = CartCacheKeyBuilder.CartCacheKeyWithUserId(cart.UserId);

            await _cachingProvider.RemoveAsync(cardCacheKey);
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Order);

            foreach (var item in updateStockEvents)
            {
                await _bus.Publish(item);
            }

            foreach (var item in updateCartItemStockEvents)
            {
                await _bus.Publish(item);
            }

            return ServiceResult<string>.Success(OrderServiceSuccess.SuccessfulCreateOrder);
        }

        return ServiceResult<string>.Failed(OrderServiceErrors.CreateOrderProblem);
    }
    public async Task<ServiceResult<string>> ChangeOrderStatusAsync(int orderId, ChangeOrderStatusDto changeOrderStatusDto)
    {
        var order = await _orderRepository.GetOrderDetailsAsync(orderId, track: true);

        if (order is null)
        {
            return ServiceResult<string>.Failed(OrderServiceErrors.OrderNotFound);
        }

        if (!order.ChangeOrderStatus(changeOrderStatusDto.OrderStatus))
        {
            return ServiceResult<string>.Failed(OrderServiceErrors.OrderStatusInvalid);
        }

        if (await _unitOfWork.SaveChangesAsync(true) > 0)
        {
            await _cachingProvider.RemoveByPrefixAsync(CacheKeysContract.Order);

            return ServiceResult<string>.Success(OrderServiceSuccess.SuccessfulChangeOrderStatus);
        }

        return ServiceResult<string>.Failed(OrderServiceErrors.ChangeOrderStatusProblem);

    }
    public async Task<ServiceResult<PagedResult<SingleOrderDto>>> GetAllOrderAsync(string userId, PagedResultOffset resultOffset)
    {
        string cacheKey = OrderCacheKeyBuilder.OrderCacheKeyWithUserId(userId, resultOffset);

        var cache = await _cachingProvider.GetAsync<PagedResult<SingleOrderDto>>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<PagedResult<SingleOrderDto>>.Success(cache.Value);
        }

        var listOfOrders = await _orderQueryRepository.GetAllOrdersAsync(userId, resultOffset);

        await _cachingProvider.TrySetAsync(cacheKey, listOfOrders, TimeSpan.FromHours(1));

        return ServiceResult<PagedResult<SingleOrderDto>>.Success(listOfOrders);
    }
    public async Task<ServiceResult<PagedResult<SingleOrderDto>>> GetAllOrderAsync(PagedResultOffset resultOffset)
    {
        string cacheKey = OrderCacheKeyBuilder.OrderPaginationCacheKey(resultOffset);

        var cache = await _cachingProvider.GetAsync<PagedResult<SingleOrderDto>>(cacheKey);

        if (cache.HasValue)
        {
            return ServiceResult<PagedResult<SingleOrderDto>>.Success(cache.Value);
        }

        var listOfOrders = await _orderQueryRepository.GetAllOrdersAsync(resultOffset);

        await _cachingProvider.TrySetAsync(cacheKey, listOfOrders, TimeSpan.FromHours(1));

        return ServiceResult<PagedResult<SingleOrderDto>>.Success(listOfOrders);
    }
}
