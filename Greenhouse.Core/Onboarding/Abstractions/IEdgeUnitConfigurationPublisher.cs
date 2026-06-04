namespace Greenhouse.Core.Onboarding.Abstractions;

public interface IEdgeUnitConfigurationPublisher
{
    Task PublishAsync(
        EdgeUnitConfiguration configuration,
        CancellationToken cancellationToken = default);
}
