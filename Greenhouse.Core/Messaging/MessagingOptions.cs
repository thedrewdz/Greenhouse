namespace Greenhouse.Core.Messaging;

public sealed class MessagingOptions
{
    public string Host { get; set; } = "localhost";

    public int Port { get; set; } = 1883;

    public string ClientId { get; set; } = "greenhouse-main-control";
}
