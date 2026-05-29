using Greenhouse.Core.Messaging;
using Greenhouse.Core.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace Greenhouse.Mqtt;

public sealed class Repository : IMessagingRepository, IAsyncDisposable
{
    private readonly MessagingOptions options;
    private readonly ILogger<Repository> logger;
    private readonly IMqttClient client;

    public Repository(
        IOptions<MessagingOptions> options,
        ILogger<Repository> logger)
    {
        this.options = options.Value;
        this.logger = logger;

        var factory = new MqttClientFactory();
        client = factory.CreateMqttClient();
        client.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;
    }

    public bool IsConnected => client.IsConnected;

    public event Func<MessageEnvelope, CancellationToken, Task>? MessageReceived;

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (client.IsConnected)
        {
            return;
        }

        var connectionOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(options.Host, options.Port)
            .WithClientId(options.ClientId)
            .WithCleanSession()
            .Build();

        await client.ConnectAsync(connectionOptions, cancellationToken);

        logger.LogInformation(
            "Connected to MQTT broker {Host}:{Port} as {ClientId}.",
            options.Host,
            options.Port,
            options.ClientId);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (!client.IsConnected)
        {
            return;
        }

        await client.DisconnectAsync(cancellationToken: cancellationToken);
    }

    public async Task SubscribeAsync(string topic, CancellationToken cancellationToken = default)
    {
        await client.SubscribeAsync(topic, cancellationToken: cancellationToken);
        logger.LogInformation("Subscribed to topic {Topic}.", topic);
    }

    public async Task PublishAsync(
        string topic,
        string payload,
        CancellationToken cancellationToken = default)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await client.PublishAsync(message, cancellationToken);
        logger.LogInformation("Messaging TX [{Topic}] {Payload}", topic, payload);
    }

    public async ValueTask DisposeAsync()
    {
        client.ApplicationMessageReceivedAsync -= OnMessageReceivedAsync;

        if (client.IsConnected)
        {
            await client.DisconnectAsync();
        }

        client.Dispose();
    }

    private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        var payload = args.ApplicationMessage.ConvertPayloadToString();
        var envelope = new MessageEnvelope(args.ApplicationMessage.Topic, payload);
        var handler = MessageReceived;

        if (handler is null)
        {
            return;
        }

        await handler(envelope, CancellationToken.None);
    }
}
