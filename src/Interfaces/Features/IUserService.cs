using PersonalShop.Domain.Response;
using PersonalShop.Domain.Users;

namespace PersonalShop.Interfaces.Features
{
    public interface IUserService
    {
        Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail, string roleName);
        Task<bool> CheckUserExistAsync(string userEmail);
        Task<ServiceResult<string>> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber);
        Task<ServiceResult<List<string>>> GetUserRolesByIdAsync(string userId);
        Task<ServiceResult<string>> RemoveUserRoleByEmailAsync(string userEmail, string roleName);
    }
}