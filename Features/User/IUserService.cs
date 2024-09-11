using PersonalShop.Domain.Users;

namespace PersonalShop.Features.User
{
    public interface IUserService
    {
        string CreateTokenAsync(Domain.Users.User user);
        Task<bool> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber);
    }
}