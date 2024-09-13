using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PersonalShop.Interfaces.Features;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalShop.Features.User;

public class UserService : IUserService
{
    private readonly UserManager<Domain.Users.User> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<Domain.Users.User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber)
    {
        var user = new Domain.Users.User { UserName = userName, Email = email };

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            user.SetPersonalName(firstName, lastName);
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            user.PhoneNumber = phoneNumber;
        }

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return false;
        }

        return true;
    }

    public string CreateTokenAsync(Domain.Users.User user)
    {
        IEnumerable<Claim> _claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:key").Value!));

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(claims: _claims,
        expires: DateTime.UtcNow.AddMinutes(60),
        signingCredentials: signingCredentials,
        issuer: _configuration.GetSection("JWT:Issuer").Value,
        audience: _configuration.GetSection("JWT:Audience").Value);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return tokenString;
    }
}

