#include <Arduino.h>
#include <WiFi.h>
#include <PubSubClient.h>

const char* WIFI_SSID = "Ryloth";
const char* WIFI_PASSWORD = "TheBas3ment";

const char* MQTT_HOST = "192.168.4.94";
const int MQTT_PORT = 1883;

const char* DEVICE_ID = "esp32-test-01";

const char* DISCOVERY_TOPIC = "greenhouse/discovery/announce";
const char* RESPONSE_TOPIC = "greenhouse/discovery/response/esp32-test-01";
const char* COMMAND_TOPIC = "greenhouse/devices/esp32-test-01/commands";
const char* HEARTBEAT_TOPIC = "greenhouse/devices/esp32-test-01/heartbeat";

String deviceId;
String responseTopic;
String commandTopic;
String heartbeatTopic;
String telemetryTopic;
String stateTopic;

WiFiClient wifiClient;
PubSubClient mqtt(wifiClient);

unsigned long lastHeartbeat = 0;

String getMacBasedDeviceId() {
  uint8_t mac[6];
  WiFi.macAddress(mac);

  char id[20];
  snprintf(
    id,
    sizeof(id),
    "esp32-%02x%02x%02x%02x%02x%02x",
    mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]
  );

  return String(id);
}

void onMqttMessage(char* topic, byte* payload, unsigned int length) {
  Serial.print("MQTT message on topic: ");
  Serial.println(topic);

  Serial.print("Payload: ");
  for (unsigned int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
  Serial.println();
}

void connectWiFi() {
  Serial.print("Connecting to WiFi");

  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println();
  Serial.print("WiFi connected. ESP32 IP: ");
  Serial.println(WiFi.localIP());
}

void initializeDeviceIdentity() {
  deviceId = getMacBasedDeviceId();

  String baseTopic = "greenhouse/devices/" + deviceId;

  responseTopic = "greenhouse/discovery/response/" + deviceId;
  commandTopic = baseTopic + "/commands";
  heartbeatTopic = baseTopic + "/heartbeat";
  telemetryTopic = baseTopic + "/telemetry";
  stateTopic = baseTopic + "/state";

  Serial.print("Device ID: ");
  Serial.println(deviceId);
}

void publishDiscoveryAnnouncement() {
  String payload = "{";
  payload += "\"deviceId\":\"";
  payload += deviceId;
  payload += "\",";
  payload += "\"deviceType\":\"sensor-node\",";
  payload += "\"firmware\":\"0.1.0\",";
  payload += "\"ip\":\"";
  payload += WiFi.localIP().toString();
  payload += "\",";
  payload += "\"capabilities\":[\"test-heartbeat\",\"mqtt-command\"],";
  payload += "\"responseTopic\":\"";
  payload += responseTopic;
  payload += "\"";
  payload += "}";

  bool published = mqtt.publish(DISCOVERY_TOPIC, payload.c_str());

  Serial.print("Discovery publish: ");
  Serial.println(published ? "OK" : "FAILED");

  Serial.print("Discovery payload: ");
  Serial.println(payload);
}

void connectMQTT() {
  while (!mqtt.connected()) {
    Serial.print("Connecting to MQTT... ");

    if (mqtt.connect(DEVICE_ID)) {
      Serial.println("connected");

      mqtt.subscribe(COMMAND_TOPIC);
      Serial.println("Subscribed to command topic");

      mqtt.subscribe(RESPONSE_TOPIC);
      Serial.print("Subscribed to response topic: ");
      Serial.println(RESPONSE_TOPIC);

      publishDiscoveryAnnouncement();      
    } else {
      Serial.print("failed, rc=");
      Serial.println(mqtt.state());
      delay(2000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  delay(1000);

  connectWiFi();
  initializeDeviceIdentity();

  mqtt.setServer(MQTT_HOST, MQTT_PORT);
  mqtt.setBufferSize(1024);
  mqtt.setCallback(onMqttMessage);

  connectMQTT();
}

void loop() {
  if (!mqtt.connected()) {
    connectMQTT();
  }

  mqtt.loop();

  if (millis() - lastHeartbeat > 5000) {
    lastHeartbeat = millis();

    mqtt.publish(
      HEARTBEAT_TOPIC,
      "online"
    );

    Serial.println("Published heartbeat");
  }
  delay(10);
}