using PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail, string roleName);
    Task<bool> CheckUserExistAsync(string userEmail);
    Task<ServiceResult<string>> CreateUserAsync(RegisterDto registerDto);
    Task<ServiceResult<List<string>>> GetUserRolesByIdAsync(string userId);
    Task<ServiceResult<string>> RemoveUserRoleByEmailAsync(string userEmail, string roleName);
}