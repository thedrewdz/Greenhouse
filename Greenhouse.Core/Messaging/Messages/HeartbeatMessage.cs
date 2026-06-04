using System.Text.Json.Serialization;

namespace Greenhouse.Core.Messaging.Messages;

public sealed record HeartbeatMessage
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("device_id")]
    public required string DeviceId { get; init; }

    [JsonPropertyName("hardware_revision")]
    public string? HardwareRevision { get; init; }

    [JsonPropertyName("firmware_version")]
    public string? FirmwareVersion { get; init; }

    [JsonPropertyName("uptime_seconds")]
    public long UptimeSeconds { get; init; }

    [JsonPropertyName("wifi_rssi")]
    public int WifiRssi { get; init; }

    [JsonPropertyName("slot_count")]
    public int? SlotCount { get; init; }

    [JsonPropertyName("slots")]
    public IReadOnlyList<HeartbeatSlotMessage> Slots { get; init; } = Array.Empty<HeartbeatSlotMessage>();

    [JsonPropertyName("capabilities")]
    public IReadOnlyList<string> Capabilities { get; init; } = Array.Empty<string>();
}

public sealed record HeartbeatSlotMessage
{
    [JsonPropertyName("slot_id")]
    public int SlotId { get; init; }

    [JsonPropertyName("direction")]
    public required string Direction { get; init; }

    [JsonPropertyName("i2c_address")]
    public required string I2cAddress { get; init; }

    [JsonPropertyName("capability")]
    public required string Capability { get; init; }

    [JsonPropertyName("state")]
    public required string State { get; init; }

    [JsonPropertyName("value")]
    public decimal Value { get; init; }

    [JsonPropertyName("error_code")]
    public int ErrorCode { get; init; }
}
