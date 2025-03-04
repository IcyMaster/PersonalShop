using PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<string>> CreateUserAsync(RegisterDto registerDto);
    Task<ServiceResult<PagedResult<SingleUserDto>>> GetAllUsersAsync(PagedResultOffset resultOffset);
    Task<ServiceResult<List<string>>> GetUserRolesByIdAsync(string userId);
    Task<ServiceResult<string>> RemoveUserRoleByEmailAsync(string userEmail, string roleName);
    Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail, string roleName);
    Task<ServiceResult<List<string>>> GetUserRolesByEmailAsync(string userEmail);
    Task<ServiceResult<SingleUserDto>> GetUserByEmailAsync(string userEmail);
}