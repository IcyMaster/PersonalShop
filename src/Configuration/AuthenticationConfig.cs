
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using PersonalShop.Data;
using PersonalShop.Domain.Roles;
using PersonalShop.Domain.Users;
using System.Text;

namespace PersonalShop.Configuration;

public static class AuthenticationConfig
{
    public static void RegisterIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, UserRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredLength = 5;
        });
    }

    public static void RegisterJwtAndMultiAuthPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JWT_OR_COOKIE";
            options.DefaultScheme = "JWT_OR_COOKIE";
            options.DefaultChallengeScheme = "JWT_OR_COOKIE";
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromHours(10);
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
                string? authorization = context.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    // otherwise always check for cookie auth
                    return JwtBearerDefaults.AuthenticationScheme;
                }
                else
                {
                    return IdentityConstants.ApplicationScheme;
                }
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<SignInManager<User>>();
    }
}
