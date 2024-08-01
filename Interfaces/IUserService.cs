using PersonalShop.Domain.Users;
using PersonalShop.Domain.Users.DTO;
using System.Security.Claims;

namespace PersonalShop.Interfaces
{
    public interface IUserService
    {
        string CreateTokenAsync(CustomUser user);
        Task<bool> CreateUserAsync(string userName, string password, string email, string? firstName, string? lastName, string? phoneNumber);
        Task<CustomUser> GetUserAsync(ClaimsPrincipal userIdentity);
    }
}