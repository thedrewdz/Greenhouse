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

    [JsonPropertyName("capabilities")]
    public IReadOnlyList<string> Capabilities { get; init; } = Array.Empty<string>();
}
