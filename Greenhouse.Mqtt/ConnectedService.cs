using Greenhouse.Core.Messaging;
using Greenhouse.Core.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Greenhouse.Mqtt;

public sealed class ConnectedService(
    IMessagingRepository repository,
    IMessageRouter messageRouter,
    ILogger<ConnectedService> logger) : BackgroundService, IConnectedService
{
    public bool IsConnected => repository.IsConnected;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        repository.MessageReceived += RouteMessageAsync;
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        repository.MessageReceived -= RouteMessageAsync;
        await repository.DisconnectAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await repository.ConnectAsync(stoppingToken);
            await repository.SubscribeAsync(MessagingTopics.Heartbeat, stoppingToken);
            await repository.SubscribeAsync(MessagingTopics.Acknowledge, stoppingToken);
            await repository.SubscribeAsync(MessagingTopics.ReadResponse, stoppingToken);

            logger.LogInformation("Messaging connected and subscribed.");
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Messaging service failed during startup.");
        }
    }

    private async Task RouteMessageAsync(MessageEnvelope message, CancellationToken cancellationToken)
    {
        try
        {
            await messageRouter.RouteAsync(message.Topic, message.Payload, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to route message from topic {Topic}.", message.Topic);
        }
    }
}
