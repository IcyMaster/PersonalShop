using System.Security.Claims;
using System.Security.Principal;

namespace PersonalShop.Extension
{
    public static class IdentityExtension
    {
        public static string GetUserId(this IIdentity identity)
        {
            var claims = (ClaimsIdentity)identity;
            return claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
