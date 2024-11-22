using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Tags;

namespace PersonalShop.Interfaces.Repositories;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetTagDetailsWithoutUserAsync(int tagId, bool track = true);
}