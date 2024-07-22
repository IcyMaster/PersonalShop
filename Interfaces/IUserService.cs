using Personal_Shop.Domain.Users;
using System.Security.Claims;

namespace Personal_Shop.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(string userName, string password, string email, string? firstName, string? lastName, string? phoneNumber);
        Task<CustomUser> GetUserAsync(ClaimsPrincipal userIdentity);
    }
}