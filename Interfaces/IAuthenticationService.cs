using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces
{
    public interface IAuthenticationService
    {
        Task<CustomUser?> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}