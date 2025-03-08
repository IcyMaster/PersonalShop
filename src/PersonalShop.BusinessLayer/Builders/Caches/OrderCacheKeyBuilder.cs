using PersonalShop.Domain.Contracts;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.BusinessLayer.Builders.Caches;

public class OrderCacheKeyBuilder
{
    public static string OrderCacheKeyWithUserId(string userId, PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Order}:userId:{userId}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
    public static string OrderPaginationCacheKey(PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Order}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
}
