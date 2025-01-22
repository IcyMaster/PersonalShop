using Microsoft.EntityFrameworkCore;
using PersonalShop.BusinessLayer.Common.Interfaces;
using PersonalShop.BusinessLayer.Generator;
using PersonalShop.BusinessLayer.Generator.Interfaces;
using PersonalShop.BusinessLayer.Services.Carts;
using PersonalShop.BusinessLayer.Services.Categories;
using PersonalShop.BusinessLayer.Services.Identitys.Authentications;
using PersonalShop.BusinessLayer.Services.Identitys.Roles;
using PersonalShop.BusinessLayer.Services.Identitys.Users;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Orders;
using PersonalShop.BusinessLayer.Services.Products;
using PersonalShop.BusinessLayer.Services.Tags;
using PersonalShop.DataAccessLayer;
using PersonalShop.DataAccessLayer.Contracts;
using PersonalShop.DataAccessLayer.Repositories;
using PersonalShop.DataAccessLayer.Seeders;
using PersonalShop.Middleware;

namespace PersonalShop.Configuration;

internal static class ServicesConfig
{
    public static void RegisterExternalServices(this WebApplicationBuilder builder)
    {
        //init service interface
        var services = builder.Services;

        //db context
        services.AddDbContext<ApplicationDbContext>(ops => ops.UseSqlServer(builder.Configuration.GetConnectionString("PersonalShop") ?? throw new InvalidOperationException("Connection string 'PersonalShop' not found.")));

        //repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductQueryRepository, ProductRepository>();

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartQueryRepository, CartRepository>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderQueryRepository, OrderRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryQueryRepository, CategoryRepository>();

        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITagQueryRepository, TagRepository>();

        //services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITagService, TagService>();

        //Generators
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        //Seeders
        services.AddTransient<IDataBaseSeeder, RoleSeeder>();
        services.AddTransient<IDataBaseSeeder, OwnerSeeder>();

        //Other services
        services.AddExceptionHandler<ExceptionHelper>();
        services.AddProblemDetails();

        //Cache Service
        services.AddEasyCaching(options =>
        {
            options.UseInMemory();
        });
    }
}
