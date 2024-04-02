using Personal_Shop.Models.Identity;

namespace Personal_Shop.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(string userName, string password, string email, string? firstName, string? lastName, string? phoneNumber);
        Task<CustomUser?> GetCustomUserDetailAsync(string userId);
    }
}