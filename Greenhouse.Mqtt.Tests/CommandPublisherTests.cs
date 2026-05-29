using Greenhouse.Core.Messaging;
using Greenhouse.Core.Messaging.Abstractions;

namespace Greenhouse.Mqtt.Tests;

public sealed class CommandPublisherTests
{
    [Fact]
    public async Task PublishReadCommandAsync_PublishesExpectedTopicAndPayload()
    {
        var repository = new CapturingRepository();
        var publisher = new CommandPublisher(repository);

        await publisher.PublishReadCommandAsync("1ADD5912AF61", 5);

        Assert.Equal("ghcmd/rd-1ADD5912AF61", repository.Topic);
        Assert.Equal("{\"slot_id\":5}", repository.Payload);
    }

    [Fact]
    public async Task PublishWriteCommandAsync_PublishesExpectedTopicAndPayload()
    {
        var repository = new CapturingRepository();
        var publisher = new CommandPublisher(repository);

        await publisher.PublishWriteCommandAsync("F11234AABC1A", 4, "on");

        Assert.Equal("ghcmd/wr-F11234AABC1A", repository.Topic);
        Assert.Equal("{\"slot_id\":4,\"state\":\"on\"}", repository.Payload);
    }

    private sealed class CapturingRepository : IMessagingRepository
    {
        public bool IsConnected => true;

        public string? Topic { get; private set; }

        public string? Payload { get; private set; }

        public event Func<MessageEnvelope, CancellationToken, Task>? MessageReceived
        {
            add { }
            remove { }
        }

        public Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task SubscribeAsync(string topic, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task PublishAsync(
            string topic,
            string payload,
            CancellationToken cancellationToken = default)
        {
            Topic = topic;
            Payload = payload;
            return Task.CompletedTask;
        }
    }
}
