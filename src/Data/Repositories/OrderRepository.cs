using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Orders;
using PersonalShop.Features.Orders.Dtos;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository,IOrderQueryRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<List<SingleOrderDto>> GetAllOrdersAsync(string userId)
    {
        return await _dbSet
            .Where(e => e.UserId == userId)
            .AsSplitQuery()
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
            }).ToListAsync();
    }
    public async Task<Order?> GetOrderDetailsAsync(string userId, bool track = true)
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
}
