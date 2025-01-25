using PersonalShop.Domain.Entities.Users;

namespace PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;

public class SingleUserDto
{
    public string UserId { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public List<string> UserRoles { get; set; } = new();
    public User User { get; set; } = null!;
}
