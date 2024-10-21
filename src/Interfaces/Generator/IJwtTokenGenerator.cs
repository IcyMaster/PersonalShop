namespace PersonalShop.Interfaces.Generator
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(List<string> roles, string id);
    }
}