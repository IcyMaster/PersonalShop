using PersonalShop.Domain.Contracts;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.BusinessLayer.Builders.Caches;

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
