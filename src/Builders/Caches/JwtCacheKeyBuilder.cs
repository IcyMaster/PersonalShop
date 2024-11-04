using PersonalShop.Data.Contracts;

namespace PersonalShop.Builders.Caches;

internal static class JwtCacheKeyBuilder
{
    public static string JwtCacheKeyWithUserId(string userId)
    {
        return $"{CacheKeysContract.Jwt}:userId:{userId}";
    }
}
