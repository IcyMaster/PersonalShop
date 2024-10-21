using PersonalShop.Domain.Authentication.Dtos;
using PersonalShop.Domain.Response;
using PersonalShop.Domain.Users;

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