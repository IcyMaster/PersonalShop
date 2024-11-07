using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders;
using PersonalShop.Features.Orders.Dtos;

namespace PersonalShop.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderDetailsAsync(string userId, bool track = true);
    }
}