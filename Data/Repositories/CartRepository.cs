using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Card;
using PersonalShop.Interfaces.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalShop.Data.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Cart?> GetCartByUserIdAsync(string userId, bool track = true)
    {
        var data = await _dbSet.Where(e => e.UserId == userId).FirstOrDefaultAsync();

        if (!track && data is not null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }

    //public async Task<bool> AddCartByUserId(string userId)
    //{
    //    await _context.Carts.AddAsync(new Cart { UserId = userId });

    //    if (await _context.SaveChangesAsync() > 0)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //public async Task<bool> AddCartItem(CreateCartItemDto createCartItemDto, Guid cartId)
    //{
    //    var cart = await _context.Carts.SingleOrDefaultAsync(e => e.Id == cartId);

    //    if (cart is null)
    //    {
    //        return false;
    //    }

    //    cart.CartItems.Add(new CartItem
    //    {
    //        Product = createCartItemDto.Product,
    //        ProductId = createCartItemDto.ProductId,
    //        Quanity = createCartItemDto.Quanity,
    //    });

    //    if (await _context.SaveChangesAsync() > 0)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //public async Task<bool> DeleteCartItemById(long productId, Guid cartId)
    //{
    //    var cart = await _context.Carts.SingleOrDefaultAsync(e => e.Id == cartId);
    //    if (cart is null)
    //    {
    //        return false;
    //    }

    //    cart.CartItems.RemoveAll(e => e.ProductId == productId);

    //    if (await _context.SaveChangesAsync() < 1)
    //    {
    //        return false;
    //    }

    //    return true;
    //}
}
