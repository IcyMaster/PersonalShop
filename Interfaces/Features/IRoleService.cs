namespace PersonalShop.Interfaces.Features
{
    public interface IRoleService
    {
        Task<bool> CheckRoleExistAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
    }
}