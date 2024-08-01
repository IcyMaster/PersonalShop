using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}