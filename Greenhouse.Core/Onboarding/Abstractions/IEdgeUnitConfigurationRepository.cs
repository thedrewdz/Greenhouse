namespace Greenhouse.Core.Onboarding.Abstractions;

public interface IEdgeUnitConfigurationRepository
{
    Task<IReadOnlyList<EdgeUnitConfiguration>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<EdgeUnitConfiguration?> GetByDeviceIdAsync(
        string deviceId,
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        EdgeUnitConfiguration configuration,
        CancellationToken cancellationToken = default);
}
