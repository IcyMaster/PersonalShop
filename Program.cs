using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Personal_Shop.Api;
using Personal_Shop.Configuration;
using Personal_Shop.Data;
using Personal_Shop.Domain.Users;
using System.Text;

namespace Personal_Shop;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Use Configuration Services
        builder.Services.RegisterExternalServices();

        builder.Services.AddIdentity<CustomUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredLength = 5;
        });

        builder.Services.AddAuthentication(options =>
        {
            //use Bearer as a default auth service
            options.DefaultScheme = "JWT_OR_COOKIE";
            options.DefaultChallengeScheme = "JWT_OR_COOKIE";
        })
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(10);
            options.SlidingExpiration = true;
            options.LoginPath = "/Account/Login";
            options.Cookie = new()
            {
                Name = "PersonalShop",
                HttpOnly = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                SecurePolicy = CookieSecurePolicy.Always
            };
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,

                ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
                ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:key").Value!))
            };
        })
        .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
        {
            // runs on each auth request
            options.ForwardDefaultSelector = context =>
            {
                // filter by auth type
                string authorization = context.Request.Headers[HeaderNames.Authorization]!;
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    // otherwise always check for cookie auth
                    return JwtBearerDefaults.AuthenticationScheme;
                }
                else
                {
                    return CookieAuthenticationDefaults.AuthenticationScheme;
                }
            };
        });

        builder.Services.AddScoped<SignInManager<CustomUser>>();

        //Config Cookie
        //builder.Services.SetupIdentity();

        //Config JWT
        //builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
        //{

        //    options.TokenValidationParameters = new TokenValidationParameters()
        //    {
        //        ValidateActor = true,
        //        ValidateAudience = true,
        //        ValidateIssuer = true,
        //        ValidateIssuerSigningKey = true,
        //        RequireExpirationTime = true,
        //        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        //        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:key").Value!))
        //    };
        //});

        ////config for multi aut
        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = "JWT_OR_COOKIE";
        //    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
        //})
        //    .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
        //     {
        //         // runs on each request
        //         options.ForwardDefaultSelector = context =>
        //         {
        //             // filter by auth type
        //             string authorization = context.Request.Headers[HeaderNames.Authorization];
        //             if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        //                 return "Bearer";

        //             // otherwise always check for cookie auth
        //             return "Cookies";
        //         };

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

        //important lines to active auth system
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseExceptionHandler();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.RegisterProductApis();
        app.RegisterAccountApis();

        app.Run();
    }
}
