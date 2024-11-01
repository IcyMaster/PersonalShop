using PersonalShop.Domain.Responses;
using PersonalShop.Domain.Users;
using PersonalShop.Features.Identitys.Authentications.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<User>> LoginAsync(string email, string password);
        Task<ServiceResult<TokenDto>> LoginAsyncAndCreateToken(string email, string password);
        Task<ServiceResult<string>> LogoutApiAsync(string jwtToken);
        Task<ServiceResult<string>> LogoutAsync();
    }
}