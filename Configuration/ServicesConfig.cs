using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Data.Contracts;
using PersonalShop.Data.Repositories;
using PersonalShop.Features.Authentication;
using PersonalShop.Features.Cart;
using PersonalShop.Features.Order;
using PersonalShop.Features.Product;
using PersonalShop.Features.User;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Middleware;
using System.Globalization;

namespace PersonalShop.Configuration;

internal static class ServicesConfig
{
    public static void RegisterExternalServices(this IServiceCollection services)
    {
        //db context
        services.AddDbContext<ApplicationDbContext>(ops => ops.UseSqlite(@"Data Source=Data/DataBase/DataBase.db"));

        //repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository,OrderRepository>();

        //services
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();

        //other services
        services.AddExceptionHandler<ExceptionHelper>();
        services.AddProblemDetails();
        services.AddHttpClient();
    }
}
