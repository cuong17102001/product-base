using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RabbitMQ;

public static class AddConnectRabbitMqServer
{
    public static void AddRabbitMqServices(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });

    }
}