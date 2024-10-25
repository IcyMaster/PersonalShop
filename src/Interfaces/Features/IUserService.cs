using PersonalShop.Domain.Response;
using PersonalShop.Domain.Users.Dtos;

namespace PersonalShop.Interfaces.Features;

public interface IUserService
{
    Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail, string roleName);
    Task<bool> CheckUserExistAsync(string userEmail);
    Task<ServiceResult<string>> CreateUserAsync(RegisterDto registerDto);
    Task<ServiceResult<List<string>>> GetUserRolesByIdAsync(string userId);
    Task<ServiceResult<string>> RemoveUserRoleByEmailAsync(string userEmail, string roleName);
}