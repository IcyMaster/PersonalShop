using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Card;
using PersonalShop.Domain.Orders;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Order?> GetOrderByUserIdAsync(string userId, bool track = true)
    {
        var data = await _dbSet.Where(e => e.UserId == userId)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.Product)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<IEnumerable<Order>?> GetOrderslistByUserIdAsync(string userId)
    {
        var orders = await _dbSet.Where(e => e.UserId == userId)
            .Include(e => e.User)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.Product)
            .AsNoTracking()
            .ToListAsync();

        return orders;
    }
}
