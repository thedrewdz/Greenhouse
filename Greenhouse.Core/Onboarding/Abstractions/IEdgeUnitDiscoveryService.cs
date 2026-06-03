namespace Greenhouse.Core.Onboarding.Abstractions;

public interface IEdgeUnitDiscoveryService
{
    Task<EdgeUnitScanResult> ScanAsync(
        TimeSpan duration,
        CancellationToken cancellationToken = default);
}
