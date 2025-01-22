namespace PersonalShop.BusinessLayer.Generator.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(List<string> roles, string id);
    }
}