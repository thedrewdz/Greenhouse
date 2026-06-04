using Greenhouse.Core.Onboarding;
using Greenhouse.Core.Onboarding.Abstractions;
using Microsoft.Extensions.Logging;

namespace Greenhouse.Mqtt;

public sealed class LoggingEdgeUnitConfigurationPublisher(
    ILogger<LoggingEdgeUnitConfigurationPublisher> logger) : IEdgeUnitConfigurationPublisher
{
    public Task PublishAsync(
        EdgeUnitConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Edge Unit configuration for {DeviceId} was stored but not published because no canonical MQTT configuration topic is defined.",
            configuration.DeviceId);

        return Task.CompletedTask;
    }
}
