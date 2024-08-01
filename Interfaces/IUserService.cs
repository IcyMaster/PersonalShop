using PersonalShop.Domain.Users;
using PersonalShop.Domain.Users.Dtos;
using System.Security.Claims;

namespace PersonalShop.Interfaces
{
    public interface IUserService
    {
        string CreateTokenAsync(User user);
        Task<bool> CreateUserAsync(string userName, string password, string email, string? firstName, string? lastName, string? phoneNumber);
        Task<User> GetUserAsync(ClaimsPrincipal userIdentity);
    }
}