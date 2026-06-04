using Greenhouse.Core.Messaging;
using Greenhouse.Core.Messaging.Abstractions;
using Greenhouse.Core.Messaging.Messages;
using Greenhouse.Core.Onboarding;
using Microsoft.Extensions.Logging;

namespace Greenhouse.Mqtt;

public sealed class EdgeUnitMessageRouter(
    EdgeUnitConfigurationApplicationService edgeUnitConfigurationService,
    ILogger<EdgeUnitMessageRouter> logger) : IMessageRouter
{
    public async Task RouteAsync(
        string topic,
        string payload,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Messaging RX [{Topic}] {Payload}", topic, payload);

        if (!string.Equals(topic, MessagingTopics.Heartbeat, StringComparison.Ordinal))
        {
            return;
        }

        HeartbeatMessage? heartbeat;
        try
        {
            heartbeat = JsonMessageSerializer.Deserialize<HeartbeatMessage>(payload);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Ignoring malformed heartbeat payload.");
            return;
        }

        if (heartbeat is null || string.IsNullOrWhiteSpace(heartbeat.DeviceId))
        {
            logger.LogWarning("Ignoring heartbeat payload with missing device_id.");
            return;
        }

        var configuration = await edgeUnitConfigurationService.ProcessHeartbeatAsync(heartbeat, cancellationToken);
        logger.LogInformation(
            "Processed heartbeat for {DeviceId}; Edge Unit status is {Status}.",
            configuration.DeviceId,
            configuration.Status);
    }
}
