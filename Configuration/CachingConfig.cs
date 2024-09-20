namespace PersonalShop.Configuration
{
    public static class CachingConfig
    {
        public static void RegisterCachingServices(this IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                options.UseInMemory("JwtBlackList");
                options.UseInMemory("Carts");
            });
        }
    }
}
