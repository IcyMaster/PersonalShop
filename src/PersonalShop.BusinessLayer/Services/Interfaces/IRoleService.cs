namespace PersonalShop.BusinessLayer.Services.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CheckRoleExistAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
    }
}