namespace Greenhouse.Core.Onboarding.Abstractions;

public interface IEdgeUnitProvisioningService
{
    Task<EdgeUnitProvisioningResult> ProvisionAsync(
        DiscoveredEdgeUnit edgeUnit,
        EdgeUnitProvisioningPayload payload,
        CancellationToken cancellationToken = default);
}
