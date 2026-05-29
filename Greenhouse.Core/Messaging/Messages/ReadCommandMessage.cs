using System.Text.Json.Serialization;

namespace Greenhouse.Core.Messaging.Messages;

public sealed record ReadCommandMessage
{
    [JsonPropertyName("slot_id")]
    public int SlotId { get; init; }
}
