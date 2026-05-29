using Greenhouse.Core.Messaging;
using Greenhouse.Core.Messaging.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Greenhouse.Mqtt;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var messagingSection = configuration.GetSection("Messaging");

        services.Configure<MessagingOptions>(options =>
        {
            options.Host = messagingSection["Host"] ?? options.Host;
            options.ClientId = messagingSection["ClientId"] ?? options.ClientId;

            if (int.TryParse(messagingSection["Port"], out var port))
            {
                options.Port = port;
            }
        });

        services.AddSingleton<IMessagingRepository, Repository>();
        services.AddSingleton<ICommandPublisher, CommandPublisher>();
        services.AddSingleton<IMessageRouter, LoggingMessageRouter>();
        services.AddHostedService<ConnectedService>();

        return services;
    }
}
