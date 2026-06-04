namespace Greenhouse.Core.Onboarding;

public sealed record OnboardingSession(
    string DeviceId,
    DiscoveredEdgeUnit EdgeUnit,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset ExpiresAtUtc);
