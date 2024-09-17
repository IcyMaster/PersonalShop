using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Orders.Dtos;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderByUserIdAsync(string userId, bool track = true);
        Task<List<SingleOrderDto>> GetAllOrdersByUserIdAsync(string userId);
    }
}