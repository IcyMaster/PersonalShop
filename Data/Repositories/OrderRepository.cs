using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Orders;
using PersonalShop.Domain.Orders.Dtos;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Order?> GetOrderByUserIdAsync(string userId, bool track = true)
    {
        var data = await _dbSet.Where(e => e.UserId == userId)
            .Include(e => e.OrderItems)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public Task<List<SingleOrderDto>> GetAllOrdersByUserIdAsync(string userId)
    {
        var data = _dbSet.Where(e => e.UserId == userId)
            .AsSingleQuery()
            .AsNoTracking()
            .Select(ob => new SingleOrderDto
            {
                Id = ob.Id,
                UserId = ob.UserId,
                User = new OrderUserDto
                {
                    FirstName = ob.User.FirstName,
                    LastName = ob.User.LastName,
                    Email = ob.User.Email!,
                },

                TotalPrice = ob.TotalPrice,
                OrderDate = ob.OrderDate,
                OrderItems = ob.OrderItems.Select(oi => new SingleOrderItemDto
                {
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quanity = oi.Quanity,

                }).ToList()

            }).ToList();

        return Task.FromResult(data);
    }
}
