using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Card;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Cart?> GetCartByUserIdWithProductAsync(string userId, bool track)
    {
        var data = await _dbSet.Where(e => e.UserId == userId)
            .Include(e => e.CartItems)
            .ThenInclude(e => e.Product)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<Cart?> GetCartByUserIdWithOutProductAsync(string userId, bool track)
    {
        var data = await _dbSet.Where(e => e.UserId == userId)
            .Include(e => e.CartItems)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<Cart?> GetCartByCartIdWithOutProductAsync(Guid cartId, bool track)
    {
        var data = await _dbSet.Where(e => e.Id == cartId)
            .Include(e => e.CartItems)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
}
