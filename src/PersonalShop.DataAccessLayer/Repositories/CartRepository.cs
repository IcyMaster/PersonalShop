﻿using Microsoft.EntityFrameworkCore;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Services.Carts.Dtos;
using PersonalShop.Domain.Entities.Carts;

namespace PersonalShop.DataAccessLayer.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository, ICartQueryRepository
{
    public CartRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<SingleCartDto?> GetCartDetailsWithProductAsync(string userId)
    {
        return await _dbSet
            .Include(x => x.CartItems)

            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => new SingleCartDto
            {
                Id = x.Id,
                UserId = x.UserId,
                TotalPrice = x.TotalPrice,
                TotalItemCount = x.CartItems.Select(e => e.Quantity).Sum(),
                CartItems = x.CartItems.Select(e => new CartItemDto
                {
                    ProductId = e.ProductId,
                    Quantity = e.Quantity,
                    Product = new CartItemProductDto
                    {
                        Name = e.Product.Name,
                        Description = e.Product.Description,
                        Price = e.Product.Price,
                        Stock = e.Product.Stock
                    }
                }).ToList(),
            })
            .FirstOrDefaultAsync();
    }
    public async Task<Cart?> GetCartDetailsWithoutProductAsync(string userId, bool track = true)
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
    public async Task<List<Cart>> GetAllCartsWithoutProductAsync(bool track = true)
    {
        var query = _dbContext.Carts.Include(x => x.CartItems);

        if (!track)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        return await query.ToListAsync();
    }
}