namespace Greenhouse.Core.Messaging.Abstractions;

public interface IMessagingRepository
{
    bool IsConnected { get; }

    event Func<MessageEnvelope, CancellationToken, Task>? MessageReceived;

    Task ConnectAsync(CancellationToken cancellationToken = default);

    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task SubscribeAsync(string topic, CancellationToken cancellationToken = default);

    Task PublishAsync(
        string topic,
        string payload,
        CancellationToken cancellationToken = default);
}
