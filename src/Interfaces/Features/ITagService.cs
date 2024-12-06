using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Tags.Dtos;

namespace PersonalShop.Interfaces.Features
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