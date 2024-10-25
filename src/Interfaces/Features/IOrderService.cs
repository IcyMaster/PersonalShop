using PersonalShop.Domain.Orders.Dtos;
using PersonalShop.Domain.Response;

namespace PersonalShop.Interfaces.Features;

public interface IOrderService
{
    Task<ServiceResult<string>> CreateOrderByCartIdAsync(Guid cartId);
    Task<ServiceResult<List<SingleOrderDto>>> GetAllOrderByUserIdAsync(string userId);
}