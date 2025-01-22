using PersonalShop.BusinessLayer.Services.Tags.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Services.Interfaces
{
    public interface ITagService
    {
        Task<ServiceResult<string>> CreateTagAsync(string userId, CreateTagDto createTagDto);
        Task<ServiceResult<string>> DeleteTagAndValidateOwnerAsync(string userId, int tagId);
        Task<ServiceResult<string>> UpdateTagAndValidateOwnerAsync(string userId, int tagId, UpdateTagDto updateTagDto);
        Task<ServiceResult<PagedResult<SingleTagDto>>> GetAllTagsWithUserAsync(PagedResultOffset resultOffset);
        Task<ServiceResult<PagedResult<SingleTagDto>>> GetAllTagsWithUserAndValidateOwnerAsync(PagedResultOffset resultOffset, string userId);
    }
}