using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces.Features
{
    public interface IUserService
    {
        string CreateTokenAsync(User user);
        Task<bool> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber);
    }
}