using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderByUserIdAsync(string userId, bool track = true);
        Task<IEnumerable<Order>?> GetOrderslistByUserIdAsync(string userId);
    }
}