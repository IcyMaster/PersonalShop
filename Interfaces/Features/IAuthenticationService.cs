using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces.Features
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<bool> LogoutAsync(string jwtToken);
    }
}