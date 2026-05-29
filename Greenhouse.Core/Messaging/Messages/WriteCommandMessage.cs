using System.Text.Json.Serialization;

namespace Greenhouse.Core.Messaging.Messages;

public sealed record WriteCommandMessage
{
    [JsonPropertyName("slot_id")]
    public int SlotId { get; init; }

    [JsonPropertyName("state")]
    public required string State { get; init; }
}
