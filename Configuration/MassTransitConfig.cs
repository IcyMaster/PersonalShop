using MassTransit;
using PersonalShop.Domain.Orders;
using PersonalShop.Features.Cart.Consumers;

namespace PersonalShop.Configuration;

public static class MassTransitConfig
{
    public static void RegisterMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(option =>
        {
            option.AddConsumer<DeleteProductFromCarts>();

            option.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
