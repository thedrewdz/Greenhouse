using System.Text.Json.Serialization;

namespace Greenhouse.Core.Messaging.Messages;

public sealed record DeviceResponseMessage
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("device_id")]
    public required string DeviceId { get; init; }

    [JsonPropertyName("slot_id")]
    public int SlotId { get; init; }

    [JsonPropertyName("value")]
    public decimal Value { get; init; }

    [JsonPropertyName("state")]
    public string State { get; init; } = string.Empty;

    [JsonPropertyName("error")]
    public int Error { get; init; }
}
