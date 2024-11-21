using PersonalShop.Domain.Categorys;

namespace PersonalShop.Data.Repositories;

public class CategoryRepository : Repository<Category>
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) { }

}
