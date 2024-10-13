using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces.Features
{
    public interface IUserService
    {
        Task<bool> AssignUserRoleByEmailAsync(string userEmail, string roleName);
        Task<bool> CheckUserExistAsync(string userEmail);
        Task<string> CreateTokenAsync(User user);
        Task<bool> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber);
        Task<List<string>?> GetUserRolesByIdAsync(string userId);
        Task<bool> RemoveUserRoleByEmailAsync(string userEmail, string roleName);
    }
}