using Microsoft.EntityFrameworkCore;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Categorys;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Categories.Dtos;
using PersonalShop.Interfaces.Repositories;

namespace PersonalShop.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository, ICategoryQueryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Category?> GetCategoryDetailsWithoutUserAsync(int categoryId, bool track = true)
    {
        var data = await _dbSet
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        if (!track && data is not null)
        {
            _dbSet.Entry(data).State = EntityState.Detached;
        }

        return data;
    }
    public async Task<List<SingleCategoryDto>> GetAllCategoriesWithUserAsync()
    {
        return await _dbSet
            .Include(x => x.User)
            .AsNoTracking()
            .Select(x => new SingleCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParentId = x.ParentId,
                User = new CategoryUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                },
            })
            .ToListAsync();
    }
    public async Task<PagedResult<SingleCategoryDto>> GetAllCategoriesWithUserAsync(PagedResultOffset resultOffset)
    {
        var totalRecord = await _dbSet.CountAsync();

        var data = await
            _dbSet
            .Include(x => x.User)
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((resultOffset.PageNumber - 1) * resultOffset.PageSize)
            .Take(resultOffset.PageSize)
            .Select(x => new SingleCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParentId = x.ParentId,
                User = new CategoryUserDto
                {
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    IsOwner = false
                },
            })
            .ToListAsync();

        return PagedResult<SingleCategoryDto>.CreateNew(data, resultOffset, totalRecord);
    }
}
