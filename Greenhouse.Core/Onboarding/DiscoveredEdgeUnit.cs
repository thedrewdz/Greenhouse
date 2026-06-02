namespace Greenhouse.Core.Onboarding;

public sealed record DiscoveredEdgeUnit(
    string Address,
    string Name,
    int? Rssi);
