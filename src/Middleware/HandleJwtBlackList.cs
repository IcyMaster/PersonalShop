using EasyCaching.Core;
using Microsoft.Net.Http.Headers;

namespace PersonalShop.Middleware;

public class HandleJwtBlackList
{
    private readonly RequestDelegate _next;
    private readonly IEasyCachingProviderFactory _cachingfactory;

    public HandleJwtBlackList(RequestDelegate next, IEasyCachingProviderFactory cachingfactory)
    {
        _next = next;
        _cachingfactory = cachingfactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = context.Request.Headers[HeaderNames.Authorization];

        if (!string.IsNullOrEmpty(token))
        {
            token = token.Replace("Bearer ", string.Empty);

            var jwtBlackList = _cachingfactory.GetCachingProvider("JwtBlackList");

            if (await jwtBlackList.GetCountAsync($"JwtBlackList:{token}") > 0)
            {
                await context.Response.WriteAsJsonAsync("Please Login and get new token");
                return;
            }
        }

        await _next(context);
    }
}
