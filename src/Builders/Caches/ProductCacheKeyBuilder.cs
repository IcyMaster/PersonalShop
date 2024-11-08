using PersonalShop.Data.Contracts;

namespace PersonalShop.Builders.Caches;

internal class ProductCacheKeyBuilder
{
    public static string ProductPaginationCacheKeyWithUserId(string userId, PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Product}:userId:{userId}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
    public static string ProductPaginationCacheKey(PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Product}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
}
