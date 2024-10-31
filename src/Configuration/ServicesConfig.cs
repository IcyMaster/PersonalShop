using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Data.Contracts;
using PersonalShop.Data.Repositories;
using PersonalShop.Features.Cart;
using PersonalShop.Features.Identity.Authentication;
using PersonalShop.Features.Identity.Role;
using PersonalShop.Features.Identity.User;
using PersonalShop.Features.Order;
using PersonalShop.Features.Product;
using PersonalShop.Generator;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Generator;
using PersonalShop.Interfaces.Repositories;
using PersonalShop.Middleware;

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
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        //services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();

        //Generators
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        //other services
        services.AddExceptionHandler<ExceptionHelper>();
        services.AddProblemDetails();
    }
}
