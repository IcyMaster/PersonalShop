using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Personal_Shop.Configuration
{
    internal static class JwtAuthorization
    {
        public static void SetupJwt(this IServiceCollection services)
        {
            //services.AddAuthentication().AddJwtBearer(options =>
            //{
            //    //options.TokenValidationParameters = new TokenValidationParameters
            //    //{
            //    //    ValidateIssuer = true,
            //    //    ValidateAudience = true,
            //    //    ValidateIssuerSigningKey = true,
            //    //    ValidIssuer = "personalShopIssuer",
            //    //    ValidAudience = "personalShopAudience",
            //    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
            //    //};
            //});
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddAuthentication().AddJwtBearer().AddJwtBearer("LocalAuthIssuer");
        }
        public static string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
