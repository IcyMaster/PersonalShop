using Microsoft.AspNetCore.Identity;
using Personal_Shop.Data;
using Personal_Shop.Domain.Users;

namespace Personal_Shop.Configuration;

internal static class IdentityConfig
{
    public static void SetupIdentity(this IServiceCollection services)
    {
        services.AddIdentity<CustomUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredLength = 5;
        });

        //services.AddSession(options =>
        //{
        //    options.Cookie.Name = "PersonalShop";
        //    options.IdleTimeout = TimeSpan.FromMinutes(30);
        //    //options.Cookie.IsEssential = true;

        //    options.Cookie.HttpOnly = true;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //    options.Cookie.SameSite = SameSiteMode.Lax;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //});

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(10);
            options.SlidingExpiration = true;
            options.LoginPath = "/Account/Login";
            options.Cookie = new()
            {
                Name = "PersonalShop",
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                SecurePolicy = CookieSecurePolicy.Always
            };
        });

        services.AddScoped<SignInManager<CustomUser>>();

        //important lines
        services.AddAuthentication();
        services.AddAuthorization();
    }
}
