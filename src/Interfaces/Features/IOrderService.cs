using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Orders.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface IOrderService
{
    Task<ServiceResult<string>> CreateOrderAsync(string userId);
    Task<ServiceResult<PagedResult<SingleOrderDto>>> GetAllOrderAsync(string userId, PagedResultOffset resultOffset);
}