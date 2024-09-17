using PersonalShop.Domain.Orders.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface IOrderService
    {
        Task<bool> CreateOrderByCartIdAsync(Guid cartId);
        Task<List<SingleOrderDto>> GetAllOrderByUserIdAsync(string userId);
    }
}