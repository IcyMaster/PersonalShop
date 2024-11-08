using Microsoft.EntityFrameworkCore;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository, IProductQueryRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<SingleProductDto?> GetProductDetailsWithUserAsync(int productId)
    {
        return await
            _dbSet.Include(x => x.User)
            .AsNoTracking()
            .Where(x => x.Id == productId)
            .Select(x => new SingleProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                User = new ProductUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                }
            })
            .FirstOrDefaultAsync();
    }
    public async Task<SingleProductDto?> GetProductDetailsWithoutUserAsync(int productId)
    {
        return await
            _dbSet
            .AsNoTracking()
            .Where(x => x.Id == productId)
            .Select(x => new SingleProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                User = new ProductUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                }
            })
            .FirstOrDefaultAsync();
    }
    public async Task<PagedResult<SingleProductDto>> GetAllProductsWithUserAsync(PagedResultOffset resultOffset)
    {
        var totalRecord = await _dbSet.CountAsync();

        var data = await
            _dbSet
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((resultOffset.PageNumber - 1) * resultOffset.PageSize)
            .Take(resultOffset.PageSize)
            .Select(ob => new SingleProductDto
            {
                Id = ob.Id,
                Name = ob.Name,
                Description = ob.Description,
                Price = ob.Price,

                User = new ProductUserDto
                {
                    UserId = ob.User.Id,
                    FirstName = ob.User.FirstName,
                    LastName = ob.User.LastName,
                    IsOwner = false,
                }
            }).ToListAsync();

        return PagedResult<SingleProductDto>.CreateNew(data, resultOffset, totalRecord);
    }

    public async Task<Product?> GetProductDetailsWithUserAsync(int productId, bool track = true)
    {
        var data = await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.Id.Equals(productId));

        if (!track && data is not null)
        {
            _dbSet.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<Product?> GetProductDetailsWithoutUserAsync(int productId, bool track = true)
    {
        var data = await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(productId));

        if (!track && data is not null)
        {
            _dbSet.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
}
