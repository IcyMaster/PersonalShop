﻿using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Card;
using PersonalShop.Interfaces.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalShop.Data.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Cart?> GetCartByUserIdWithProductAsync(string userId, bool track = true)
    {
        var data = await _dbSet.Where(x => x.UserId == userId)
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<Cart?> GetCartByUserIdWithOutProductAsync(string userId, bool track = true)
    {
        var data = await _dbSet.Where(x => x.UserId == userId)
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<Cart?> GetCartByCartIdWithOutProductAsync(Guid cartId, bool track = true)
    {
        var data = await _dbSet.Where(x => x.Id == cartId)
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<List<Cart>> GetAllCartsWithOutProductAsync()
    {
        return await _dbSet.Include(x => x.CartItems).ToListAsync();
    }
}
