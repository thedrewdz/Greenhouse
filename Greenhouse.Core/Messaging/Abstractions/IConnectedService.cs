namespace Greenhouse.Core.Messaging.Abstractions;

public interface IConnectedService
{
    bool IsConnected { get; }

    Task StartAsync(CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);
}
