using PersonalShop.Shared.Contracts;

namespace PersonalShop.BusinessLayer.Builders.Caches;

public static class JwtCacheKeyBuilder
{
    public static string JwtCacheKeyWithUserId(string userId)
    {
        return $"{CacheKeysContract.Jwt}:userId:{userId}";
    }
}
