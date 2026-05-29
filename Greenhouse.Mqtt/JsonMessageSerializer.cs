using System.Text.Json;

namespace Greenhouse.Mqtt;

public static class JsonMessageSerializer
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static string Serialize<TMessage>(TMessage message)
    {
        return JsonSerializer.Serialize(message, JsonOptions);
    }

    public static TMessage? Deserialize<TMessage>(string payload)
    {
        return JsonSerializer.Deserialize<TMessage>(payload, JsonOptions);
    }
}
