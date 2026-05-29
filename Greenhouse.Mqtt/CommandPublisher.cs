using Greenhouse.Core.Messaging;
using Greenhouse.Core.Messaging.Abstractions;
using Greenhouse.Core.Messaging.Messages;

namespace Greenhouse.Mqtt;

public sealed class CommandPublisher(IMessagingRepository repository) : ICommandPublisher
{
    public Task PublishReadCommandAsync(
        string deviceId,
        int slotId,
        CancellationToken cancellationToken = default)
    {
        var message = new ReadCommandMessage { SlotId = slotId };

        return repository.PublishAsync(
            MessagingTopics.ReadCommand(deviceId),
            JsonMessageSerializer.Serialize(message),
            cancellationToken);
    }

    public Task PublishWriteCommandAsync(
        string deviceId,
        int slotId,
        string state,
        CancellationToken cancellationToken = default)
    {
        var message = new WriteCommandMessage
        {
            SlotId = slotId,
            State = state
        };

        return repository.PublishAsync(
            MessagingTopics.WriteCommand(deviceId),
            JsonMessageSerializer.Serialize(message),
            cancellationToken);
    }
}
