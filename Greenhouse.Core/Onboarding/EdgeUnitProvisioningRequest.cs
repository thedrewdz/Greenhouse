namespace Greenhouse.Core.Onboarding;

public sealed record EdgeUnitProvisioningRequest(
    DiscoveredEdgeUnit EdgeUnit,
    string DeviceId,
    string WifiSsid,
    string WifiPassword,
    Uri MqttBrokerUri,
    int? HeartbeatIntervalMs);
