using Greenhouse.Core.Onboarding;
using Greenhouse.Core.Onboarding.Abstractions;

namespace Greenhouse.Bluetooth;

public sealed class UnavailableEdgeUnitProvisioningService : IEdgeUnitProvisioningService
{
    public Task<EdgeUnitProvisioningResult> ProvisionAsync(
        DiscoveredEdgeUnit edgeUnit,
        EdgeUnitProvisioningPayload payload,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(EdgeUnitProvisioningResult.Failure(
            2099,
            "BLE provisioning transport is not configured on this Main Unit."));
    }
}
