using PersonalShop.Domain.Users;

namespace PersonalShop.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<Domain.Users.User?> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<bool> LogoutAsync(string jwtToken);
    }
}