namespace Personal_Shop.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}