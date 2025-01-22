using PersonalShop.Domain.Contracts;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.BusinessLayer.Builders.Caches;

static internal class CategoryCacheKeyBuilder
{
    public static string CategoryPaginationCacheKeyWithUserId(string userId, PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Category}:userId:{userId}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
    public static string CategoryPaginationCacheKey(PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Category}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
}
