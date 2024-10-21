namespace PersonalShop.Domain.Authentication.Dtos;

public class TokenDto
{
    public TokenDto(string jwtToken)
    {
        JwtToken = jwtToken;
    }

    public string JwtToken { get; internal set; } = string.Empty;
}
