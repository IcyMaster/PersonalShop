using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PersonalShop.BusinessLayer.Generator.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalShop.BusinessLayer.Generator;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(List<string> roles, string id)
    {
        List<Claim> _claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,id.ToString()),
        };

        foreach (var role in roles)
        {
            _claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

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
