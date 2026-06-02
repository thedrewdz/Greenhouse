namespace Greenhouse.Core.Onboarding;

public sealed record EdgeUnitScanResult(
    IReadOnlyList<DiscoveredEdgeUnit> EdgeUnits,
    bool Succeeded,
    string? ErrorMessage)
{
    public static EdgeUnitScanResult Success(IReadOnlyList<DiscoveredEdgeUnit> edgeUnits)
    {
        return new EdgeUnitScanResult(edgeUnits, true, null);
    }

    public static EdgeUnitScanResult Failure(string errorMessage)
    {
        return new EdgeUnitScanResult(Array.Empty<DiscoveredEdgeUnit>(), false, errorMessage);
    }
}
