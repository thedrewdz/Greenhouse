using System.Text;
using System.Text.Json;
using Experiment4.Model;
using MQTTnet;

const string TOPIC_ANNOUNCE = "greenhouse/discovery/announce";
const string TOPIC_RESPONSE = "greenhouse/discovery/response";
const string TOPIC_HEARTBEAT = "greenhouse/system/main-control/heartbeat";

var mqttFactory = new MqttClientFactory();
using var mqttClient = mqttFactory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost", 1883)
    .WithClientId("greenhouse-main-control")
    .WithCleanSession()
    .Build();

mqttClient.ApplicationMessageReceivedAsync += async e =>
{
    var topic = e.ApplicationMessage.Topic;
    if (e.ApplicationMessage == null) return;

    var payload = e.ApplicationMessage?.ConvertPayloadToString();

    Console.WriteLine($"RX [{topic}] {payload}");

    if (topic.Equals(TOPIC_ANNOUNCE))
    {
        var controller = new Controller { ControllerId = "main-pi", Status = "accepted" };
        var response = new MqttApplicationMessageBuilder()
            .WithTopic(TOPIC_RESPONSE)
            .WithPayload(JsonSerializer.Serialize(controller))
            .Build();

        await mqttClient.PublishAsync(response);

        Console.WriteLine("TX [greenhouse/discovery/response]");
    }
};

await mqttClient.ConnectAsync(options);

Console.WriteLine("Connected to MQTT broker.");

await mqttClient.SubscribeAsync("greenhouse/#");

Console.WriteLine("Subscribed to greenhouse/#");
Console.WriteLine("Press Enter to publish a heartbeat. Press Ctrl+C to exit.");

while (true)
{
    Console.ReadLine();

    var controller = new Controller { ControllerId = "main-pi", Status = "online" };
    var message = new MqttApplicationMessageBuilder()
        .WithTopic(TOPIC_HEARTBEAT)
        .WithPayload(JsonSerializer.Serialize(controller))
        .Build();

    await mqttClient.PublishAsync(message);

    Console.WriteLine("TX [greenhouse/system/main-control/heartbeat]");
}