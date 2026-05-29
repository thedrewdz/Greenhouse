namespace Greenhouse.Core.Messaging.Abstractions;

public interface ICommandPublisher
{
    Task PublishReadCommandAsync(
        string deviceId,
        int slotId,
        CancellationToken cancellationToken = default);

    Task PublishWriteCommandAsync(
        string deviceId,
        int slotId,
        string state,
        CancellationToken cancellationToken = default);
}
