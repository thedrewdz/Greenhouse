namespace Greenhouse.Core.Onboarding;

public sealed record EdgeUnitMappingRequest(
    string DeviceId,
    string Name,
    string? Location,
    IReadOnlyList<EdgeUnitSlotMappingRequest> Slots);

public sealed record EdgeUnitSlotMappingRequest(
    int SlotId,
    string Direction,
    string I2cAddress,
    string Capability,
    string? DisplayLabel);
