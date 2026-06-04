using System.Text.Json.Serialization;

namespace Greenhouse.Core.Onboarding;

public sealed record EdgeUnitProvisioningPayload
{
    [JsonPropertyName("schema_version")]
    public int SchemaVersion { get; init; } = 1;

    [JsonPropertyName("device_id")]
    public required string DeviceId { get; init; }

    [JsonPropertyName("wifi_ssid")]
    public required string WifiSsid { get; init; }

    [JsonPropertyName("wifi_password")]
    public required string WifiPassword { get; init; }

    [JsonPropertyName("mqtt_broker_uri")]
    public required string MqttBrokerUri { get; init; }

    [JsonPropertyName("heartbeat_interval_ms")]
    public int? HeartbeatIntervalMs { get; init; }
}
