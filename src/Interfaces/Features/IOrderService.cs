using PersonalShop.Domain.Responses;
using PersonalShop.Features.Orders.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface IOrderService
{
    Task<ServiceResult<string>> CreateOrderByUserIdAsync(string userId);
    Task<ServiceResult<List<SingleOrderDto>>> GetAllOrderByUserIdAsync(string userId);
}