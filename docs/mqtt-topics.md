# MQTT Topic Architecture

# Overview

MQTT is the central communication backbone of the greenhouse platform.

Topic naming should remain:
- hierarchical
- predictable
- human-readable
- scalable

---

# Topic Design Principles

## Consistency

All devices follow consistent naming conventions.

---

## Readability

Topics should remain understandable during debugging.

---

## Scalability

Topic hierarchy must support:
- multiple devices
- multiple greenhouses
- future expansion

---

# Proposed Topic Structure

## Sensor Topics
### Telemetry

`greenhouse/sensors/{deviceId}/{sensorType}`

Examples:

```
greenhouse/sensors/backwall-node-1/temperature
greenhouse/sensors/backwall-node-1/humidity
greenhouse/sensors/planter4-node/moisture
greenhouse/sensors/water-tank-node/level
```

### Status

`greenhouse/sensors/{deviceId}/status`

Example payload:

```
{
  "online": true,
  "firmwareVersion": "1.0.3",
  "uptimeSeconds": 92384,
  "wifiRssi": -61
}
```

### Heartbeat

`greenhouse/sensors/{deviceId}/heartbeat`

## Actuator Topics

### Commands

`greenhouse/actuators/{deviceId}/{actuatorId}/command`

Examples:

```
greenhouse/actuators/mainbox/pump1/command
greenhouse/actuators/mainbox/fan2/command
```

### State

`greenhouse/actuators/{deviceId}/{actuatorId}/state`

Example payload:

```
{
  "state": "on",
  "since": "2026-05-22T19:10:22Z"
}
```

### Availability

`greenhouse/actuators/{deviceId}/status`

### Events

`greenhouse/events/{eventType}`

Examples:

```
greenhouse/events/device-online
greenhouse/events/device-offline
greenhouse/events/rule-triggered
greenhouse/events/alarm
```

### Alerts

`greenhouse/alerts/{severity}`

Examples:

```
greenhouse/alerts/info
greenhouse/alerts/warning
greenhouse/alerts/critical
```

## Rules
### Rule Updates

`greenhouse/rules/update`

### Rule Events

`greenhouse/rules/events`

## OTA Updates

### Firmware Commands

`greenhouse/ota/{deviceId}/command`

### OTA Status

`greenhouse/ota/{deviceId}/status`

## Discovery
### Device Discovery
`greenhouse/discovery/register`

Example payload:

```
{
  "deviceId": "sensor-westwall-1",
  "deviceType": "sensor-node",
  "firmwareVersion": "1.0.0",
  "capabilities": [
    "temperature",
    "humidity"
  ]
}
```

## System Topics
### Broker/System Health
`greenhouse/system/status`

### Server Events
`greenhouse/system/events`

## Future Multi-Greenhouse Expansion

Future topic hierarchy may expand to:

```
greenhouse/{siteId}/sensors/
greenhouse/{siteId}/actuators/
```

Example:

`greenhouse/site-alpha/sensors/node1/temperature`

This allows scaling without redesigning topic structures.

## Payload Format

Preferred payload format:

- JSON initially

Future optimization options:

- MessagePack
- Protocol Buffers

JSON is preferred initially for:

- debugging
- readability
- development speed

## Retained Messages

Recommended retained topics:

- device status
- actuator state
- configuration

Avoid retaining:

- high-frequency telemetry

## QoS Recommendations
### QoS 0

Use for:

- frequent telemetry
- non-critical updates

Examples:

- temperature updates
- humidity telemetry
- RSSI reporting

### QoS 1

Use for:

- actuator commands
- rule changes
- important state changes
- alerts

Examples:

- pump activation
- OTA commands
- critical alarms

### QoS 2

Generally avoid unless absolutely necessary due to:

- additional overhead
- latency
- complexity

Potential use cases:

- billing systems
- transactional synchronization
- highly critical once-only workflows

Most greenhouse functionality should operate correctly using QoS 0 and QoS 1.

## Suggested Telemetry Payload Structure

Example sensor payload:

```
{
  "deviceId": "westwall-sensor-1",
  "sensorType": "temperature",
  "value": 24.7,
  "unit": "C",
  "timestamp": "2026-05-22T19:22:13Z"
}
```

## Suggested Command Payload Structure

Example actuator command:

```
{
  "command": "on",
  "durationSeconds": 20,
  "requestedBy": "rule-engine",
  "timestamp": "2026-05-22T19:23:55Z"
}
```

Suggested Rule Event Payload:

```
{
  "ruleId": "irrigation-rule-1",
  "triggered": true,
  "reason": "Moisture below threshold",
  "timestamp": "2026-05-22T19:24:18Z"
}
```

## Topic Naming Recommendations
**Use lowercase**

Recommended:

`greenhouse/sensors/westwall-node-1/temperature`

Avoid:

`GreenHouse/Sensors/WestWallNode1/Temperature`

**Avoid spaces**

Use:

- hyphens
- underscores

Avoid:

- spaces
- special characters

**Keep Topics Stable**

Avoid renaming topics frequently once devices are deployed.

Stable topic structures simplify:

- automation
- dashboards
- historical persistence
- integrations



## Future Extensions



Potential future topic groups:

```
greenhouse/vision/
greenhouse/weather/
greenhouse/energy/
greenhouse/ai/
greenhouse/cloud/
```

Examples:

```
greenhouse/vision/plant-health
greenhouse/weather/forecast
greenhouse/ai/recommendations
greenhouse/cloud/sync-status
```

These extensions should remain additive and not require breaking changes to the existing MQTT hierarchy.
