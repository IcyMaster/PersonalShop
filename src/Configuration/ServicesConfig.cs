﻿using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Data.Contracts;
using PersonalShop.Data.Repositories;
using PersonalShop.Data.Seeders;
using PersonalShop.Features.Carts;
using PersonalShop.Features.Categories;
using PersonalShop.Features.Identitys.Authentications;
using PersonalShop.Features.Identitys.Roles;
using PersonalShop.Features.Identitys.Users;
using PersonalShop.Features.Orders;
using PersonalShop.Features.Products;
using PersonalShop.Features.Tags;
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
