using Greenhouse.Core.Messaging.Messages;

namespace Greenhouse.Core.Onboarding;

public sealed record EdgeUnitConfiguration
{
    public required string DeviceId { get; init; }

    public required string Name { get; init; }

    public string? Location { get; init; }

    public EdgeUnitConfigurationStatus Status { get; init; }

    public IReadOnlyList<EdgeUnitSlotMapping> SlotMappings { get; init; } = Array.Empty<EdgeUnitSlotMapping>();

    public HeartbeatMessage? LastHeartbeat { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public DateTimeOffset UpdatedAtUtc { get; init; }
}

public sealed record EdgeUnitSlotMapping
{
    public int SlotId { get; init; }

    public required string Direction { get; init; }

    public required string I2cAddress { get; init; }

    public required string Capability { get; init; }

    public string? DisplayLabel { get; init; }
}
