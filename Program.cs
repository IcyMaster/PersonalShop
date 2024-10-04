using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using PersonalShop.Api;
using PersonalShop.Configuration;
using PersonalShop.Data;
using PersonalShop.Domain.Users;
using PersonalShop.Middleware;
using System.Globalization;
using System.Text;

namespace PersonalShop;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add controllers with views, and enable localization in views and data annotations
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

        // Add Caching services
        builder.Services.RegisterCachingServices();

        // Use Configuration Services
        builder.Services.RegisterExternalServices();

        // Add Authentication Services
        builder.RegisterIdentity();
        builder.RegisterJwtAndMultiAuthPolicy();

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<SignInManager<User>>();

        var app = builder.Build();

        // Register Localization service
        app.RegisterLocalization();

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


        app.Run();
    }
}
