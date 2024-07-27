using Personal_Shop.Domain.Users;

namespace Personal_Shop.Interfaces
{
    public interface IAuthenticationService
    {
        Task<CustomUser?> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}