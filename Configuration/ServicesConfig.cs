using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Personal_Shop.Data;
using Personal_Shop.Features.Authentication;
using Personal_Shop.Features.Product;
using Personal_Shop.Features.User;
using Personal_Shop.Interfaces;
using Personal_Shop.Middleware;
using System.Globalization;

namespace Personal_Shop.Configuration;

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
