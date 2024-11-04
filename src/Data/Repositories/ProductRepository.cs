using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Products;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<SingleProductDto?> GetProductByIdWithUserAsync(int id, bool track = true)
    {
        var data = await _dbSet.Include(e => e.User).FirstOrDefaultAsync(e => e.Id.Equals(id));

        if (!track && data is not null)
        {
            _dbSet.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<Product?> GetProductByIdWithOutUserAsync(int id, bool track = true)
    {
        var data = await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id));

        if (!track && data is not null)
        {
            _dbSet.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<List<Product>> GetAllProductsWithUserAsync(bool track = true)
    {
        var query = _dbContext.Products.Include(e => e.User);

        if (!track)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        return await query.ToListAsync();
    }
}
