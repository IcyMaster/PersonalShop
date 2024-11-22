using PersonalShop.Domain.Responses;
using PersonalShop.Features.Tags.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface ITagService
    {
        Task<ServiceResult<string>> CreateTagAsync(string userId, CreateTagDto createTagDto);
        Task<ServiceResult<string>> DeleteTagAndValidateOwnerAsync(string userId, int tagId);
        Task<ServiceResult<List<SingleTagDto>>> GetAllTagsWithUserAndValidateOwnerAsync(string userId);
        Task<ServiceResult<List<SingleTagDto>>> GetAllTagsWithUserAsync();
        Task<ServiceResult<string>> UpdateTagAndValidateOwnerAsync(string userId, int tagId, UpdateTagDto updateTagDto);
    }
}