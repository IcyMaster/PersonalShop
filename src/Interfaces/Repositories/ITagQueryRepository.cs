using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Domain.Tags;
using PersonalShop.Features.Tags.Dtos;

namespace PersonalShop.Interfaces.Repositories;

public interface ITagQueryRepository : IRepository<Tag>
{
    Task<SingleTagDto?> FindTagWithNameAsync(string name);
    Task<PagedResult<SingleTagDto>> GetAllTagsWithUserAsync(PagedResultOffset resultOffset);
}