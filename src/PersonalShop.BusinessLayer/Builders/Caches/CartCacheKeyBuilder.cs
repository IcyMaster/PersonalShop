using PersonalShop.Shared.Contracts;

namespace PersonalShop.BusinessLayer.Builders.Caches;

internal static class CartCacheKeyBuilder
{
    public static string CartCacheKeyWithUserId(string userId)
    {
        return $"{CacheKeysContract.Cart}:userId:{userId}";
    }
}
