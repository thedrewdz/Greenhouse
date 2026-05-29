namespace Greenhouse.Core.Messaging;

public static class MessagingTopics
{
    public const string Heartbeat = "gh/heartbeat";
    public const string Acknowledge = "gh/ack";
    public const string ReadResponse = "gh/rd";

    public static string ReadCommand(string deviceId) => $"ghcmd/rd-{deviceId}";

    public static string WriteCommand(string deviceId) => $"ghcmd/wr-{deviceId}";
}
