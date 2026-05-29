# MQTT Topic Architecture

# Overview

MQTT is the central communication backbone of the greenhouse platform.

Topic naming should remain:
- hierarchical
- predictable
- human-readable
- scalable

Avoid redundancy where possible to keep things simple.

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
- multiple greenhouses (each has its own control unit with paired sensor and actuator units, but could live on the same WIFI)
- future expansion

---

# Proposed Topic Structure

## Device Identification

Use WIFI Mac address as a unit's device ID
- Format: {mac_address}
- No configuration is stored on peripheral unit
- Allows main unit to maintain peripheral awareness and config.

## Command Topics

Issue an instruction to read/write to a peripheral

### Telemetry

`ghcmd/{commandType}-{deviceId}`

- `ghcmd` - This is a command topic
- `{commandType}` - `[rd|wr]`, `rd` - Read, `wr` - Write.
- `{deviceId}` - Each device listens to an instruction topic on its own device Id derived from its WIFI Mac Address
- The payload body contains the instruction
- Read instructions could request values from a specific sensor, or the unit's current status.
- Read and write commands are on separate topics due to differing payloads.

Topic Examples:

```
ghcmd/rd-1ADD5912AF61
ghcmd/rd-1ADD5912AF61
ghcmd/wr-F11234AABC1A
ghcmd/wr-3555FA1BD1EE
```

**Read Command Example:**

Requests the value of the sensor on slot 0.

```
{
  "slot_id": 0
}
```

**Write Command Example:**

Instructs the peripheral unit to turn the actuator on slot 4 on (high).

```
{
  "slot_id": 4,
  "state": "on"
}
```

## Response Topics

Peripheral Unit publishes a response payload in response to a command.

### Telemetry

`gh/{responseType}`

- `ghcmd` - This is a command topic
- `{responseType}` - `[ack|rd|heartbeat]` - `ack` - Acknowledge receipt of write command with result. `rd` - Reading from a sensor. `heartbeat` - Special case, see below.
- Acknowledge and readings may have different schemas

**Acknowledge Message:**

Reports that the relay on slot 4 has been activated.

```
{
  "id": 123,
  "device_id": "F11234AABC1A",
  "slot_id", 4,
  "value": 0,
  "state": "on",
  "error": 0
}
```

**Read Response Message:**

Returns the temperature in celcius from the temperature sensor on slot 5

```
{
  "id": 235,
  "device_id": "1ADD5912AF61",
  "slot_id", 5,
  "value": 19.2,
  "state": "",
  "error": 0
}
```

**Error Response**

- Commands can result in an error
- especially if a user misconfigures a unit during setup. 
- Error codes to be determined as needed.

**Error Example:**

Reports that the unit cannot comply with the request.

```
{
  "id": 123,
  "device_id": "F11234AABC1A",
  "slot_id", 4,
  "value": 0,
  "state": "error",
  "error_code": 1000  
}
```

## Heartbeat

- Peripheral units publish a heartbeat on a regular interval
- Also used as `is_online` status

`gh/heartbeat`

Example Payload:

```
{
  "id": 8339,
  "device_id": "1ADD5912AF61",
  "hardwareRevision": "A",
  "firmwareVersion": "1.0.3",
  "uptimeSeconds": 92384,
  "wifiRssi": -61,
  "capabilities": [
    "temperature",
    "humidity",
    "light"
  ]
}
```

# OTA Updates

In the future we want to be able to push firmware updates from the web to both the main unit and peripheral units.

## Firmware Commands

`ghota/{deviceId}/update`

## OTA Status

`ghota/{deviceId}/status`

# Payload Format

Preferred payload format:

- JSON initially

JSON is preferred initially for:

- debugging
- readability
- development speed

# Retained Messages

Recommended retained topics:

- device status
- actuator state
- configuration

Avoid retaining:

- high-frequency telemetry (heartbeats)

# Topic Naming Recommendations

- Use lowercase
- hyphens
- not underscores

Avoid:

- spaces
- special characters

## General Naming Considerations

- Proper design means backward compatibility even after devices are deployed
- Keep number of topics low - avoid redundancy
- Peripheral subscribes to: `ghcmd/rd-{deviceId}` and `ghcmd/wr-{deviceId}`
- Peripheral publishes to: `gh/heartbeat`, `gh/ack`, `gh/rd`
- Main Unit Subscribes to: `gh/heartbeat`, `gh/ack`, `gh/rd`
- Main Unit publishes to: `ghcmd/rd-{deviceId}` and `ghcmd/wr-{deviceId}`
