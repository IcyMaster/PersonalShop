using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderDetailsAsync(string userId, bool track = true);
    }
}