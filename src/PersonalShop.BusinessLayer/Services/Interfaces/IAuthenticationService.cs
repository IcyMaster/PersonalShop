using PersonalShop.BusinessLayer.Services.Identitys.Authentications.Dtos;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Domain.Entities.Users;

namespace PersonalShop.BusinessLayer.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<User>> LoginAsync(string email, string password);
        Task<ServiceResult<TokenDto>> LoginAsyncAndCreateToken(string email, string password);
        Task<ServiceResult<string>> LogoutApiAsync(string jwtToken, string userId);
        Task<ServiceResult<string>> LogoutAsync();
    }
}