using Microsoft.EntityFrameworkCore;
using PersonalShop.Data.Repositories.Interfaces;
using PersonalShop.Domain.Card;

namespace PersonalShop.Data.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Cart?> GetCartByUserId(string userId)
    {
        return await _dbSet.Where(e => e.UserId == userId).FirstOrDefaultAsync();
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
