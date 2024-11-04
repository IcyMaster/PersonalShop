using PersonalShop.Data.Contracts;

namespace PersonalShop.Builders.Caches;

internal static class CartCacheKeyBuilder
{
    public static string CartCacheKeyWithUserId(string userId)
    {
        return $"{CacheKeysContract.Cart}:userId:{userId}";
    }
}
