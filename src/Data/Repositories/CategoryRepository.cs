using Microsoft.EntityFrameworkCore;
using PersonalShop.Domain.Categorys;
using PersonalShop.Features.Categorys.Dtos;
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
    public async Task<List<SingleCategoryDto>> GetAllCategorysWithUserAsync()
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
}
