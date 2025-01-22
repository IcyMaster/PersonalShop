using EasyCaching.Core;
using Microsoft.Net.Http.Headers;
using PersonalShop.BusinessLayer.Builders.Caches;
using PersonalShop.Extension;

namespace PersonalShop.Middleware;

public class HandleJwtBlackList
{
    private readonly RequestDelegate _next;
    private readonly IEasyCachingProvider _cachingProvider;

    public HandleJwtBlackList(RequestDelegate next, IEasyCachingProvider cachingProvider)
    {
        _next = next;
        _cachingProvider = cachingProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = context.Request.Headers[HeaderNames.Authorization];

        if (!string.IsNullOrEmpty(token))
        {
            var userId = context.GetUserId();

            token = token.Replace("Bearer ", string.Empty);

            string cacheKey = JwtCacheKeyBuilder.JwtCacheKeyWithUserId(userId!);

            var cache = await _cachingProvider.GetAsync<string>(cacheKey);

            if (cache.HasValue)
            {
                await context.Response.WriteAsJsonAsync("Please Login and get new token");
                return;
            }
        }

        await _next(context);
    }
}
