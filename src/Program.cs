using Microsoft.AspNetCore.Identity;
using PersonalShop.Api;
using PersonalShop.Configuration;
using PersonalShop.Domain.Users;
using PersonalShop.Middleware;

namespace PersonalShop;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register Controller With Views
        builder.Services.AddControllersWithViews();

        // Register MassTransit and RabitMQ services
        builder.Services.RegisterMassTransit();

        // Add Caching services
        builder.Services.RegisterCachingServices();

        // Use Configuration Services
        builder.Services.RegisterExternalServices();

        // Add Authentication Services
        builder.RegisterIdentity();
        builder.RegisterJwtAndMultiAuthPolicy();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Add Middlewares
        app.UseMiddleware<HandleJwtBlackList>();

        // Register Minimal Apis
        app.RegisterAccountApis();
        app.RegisterProductApis();
        app.RegisterUserApis();
        app.RegisterCardApis();

        //important lines to active auth system
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseExceptionHandler();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        //seed all roles and create defualt owner user for first time
        app.SeedRolesAndOwnerUserAsync();

        app.Run();
    }
}