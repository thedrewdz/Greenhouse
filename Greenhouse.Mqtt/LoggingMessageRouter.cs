using Greenhouse.Core.Messaging.Abstractions;
using Microsoft.Extensions.Logging;

namespace Greenhouse.Mqtt;

public sealed class LoggingMessageRouter(ILogger<LoggingMessageRouter> logger) : IMessageRouter
{
    public Task RouteAsync(
        string topic,
        string payload,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Messaging RX [{Topic}] {Payload}", topic, payload);
        return Task.CompletedTask;
    }
}
