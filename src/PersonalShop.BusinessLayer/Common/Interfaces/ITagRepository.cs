using PersonalShop.Domain.Entities.Tags;

namespace PersonalShop.BusinessLayer.Common.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetTagDetailsWithoutUserAsync(int tagId, bool track = true);
}