using PersonalShop.Data.Contracts;

namespace PersonalShop.Builders.Caches;

public class OrderCacheKeyBuilder
{
    public static string OrderCacheKeyWithUserId(string userId, PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Order}:userId:{userId}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
}
