using Microsoft.EntityFrameworkCore;
using Personal_Shop.Data;
using Personal_Shop.Interfaces;
using Personal_Shop.Middleware;
using Personal_Shop.Services;

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
    }
}
