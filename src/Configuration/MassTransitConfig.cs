using MassTransit;
using PersonalShop.Features.Carts.Consumers;

namespace PersonalShop.Configuration;

public static class MassTransitConfig
{
    public static void RegisterMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(option =>
        {
            option.AddConsumer<DeleteProductFromCarts>();
            option.AddConsumer<UpdateProductInCarts>();

            option.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration.GetSection("RabitMq:Host").Value, "/", h =>
                {
                    h.Username(builder.Configuration.GetSection("RabitMq:UserName").Value!);
                    h.Password(builder.Configuration.GetSection("RabitMq:Password").Value!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
