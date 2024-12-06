using PersonalShop.Data.Contracts;

namespace PersonalShop.Builders.Caches;

internal static class TagCacheKeyBuilder
{
    public static string TagPaginationCacheKeyWithUserId(string userId, PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Tag}:userId:{userId}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
    public static string TagPaginationCacheKey(PagedResultOffset resultOffset)
    {
        return $"{CacheKeysContract.Tag}:pageNumber:{resultOffset.PageNumber}:pageSize:{resultOffset.PageSize}";
    }
}
