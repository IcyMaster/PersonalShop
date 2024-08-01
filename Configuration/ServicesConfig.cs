using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Features.Authentication;
using PersonalShop.Features.Product;
using PersonalShop.Features.User;
using PersonalShop.Interfaces;
using PersonalShop.Middleware;
using System.Globalization;

namespace PersonalShop.Configuration;

internal static class ServicesConfig
{
    public static void RegisterExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddDbContext<ApplicationDbContext>(ops => ops.UseSqlite(@"Data Source=Data/DataBase/DataBase.db"));
        services.AddExceptionHandler<ExceptionHelper>();
        services.AddProblemDetails();
        services.AddHttpClient();
    }
}
