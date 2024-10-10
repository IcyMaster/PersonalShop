using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces.Features
{
    public interface IUserService
    {
        Task<bool> AssignUserRoleAsync(string userEmail, string roleName);
        Task<bool> CheckUserExistAsync(string userEmail);
        string CreateToken(User user);
        Task<bool> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber);
        Task<bool> RemoveUserRoleAsync(string userEmail, string roleName);
    }
}