using PersonalShop.BusinessLayer.Services.Tags.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Domain.Entities.Tags;

namespace PersonalShop.BusinessLayer.Common.Interfaces;

public interface ITagQueryRepository : IRepository<Tag>
{
    Task<SingleTagDto?> FindTagWithNameAsync(string name);
    Task<PagedResult<SingleTagDto>> GetAllTagsWithUserAsync(PagedResultOffset resultOffset);
}