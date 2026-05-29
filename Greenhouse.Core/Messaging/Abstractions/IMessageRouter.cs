namespace Greenhouse.Core.Messaging.Abstractions;

public interface IMessageRouter
{
    Task RouteAsync(
        string topic,
        string payload,
        CancellationToken cancellationToken = default);
}
