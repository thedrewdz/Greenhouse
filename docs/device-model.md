# Device Model

# Overview

The greenhouse platform uses a distributed device model built around modular ESP32-based nodes.

Devices are categorized by capability rather than strict hardware identity.

This allows:
- flexibility
- future expansion
- dynamic discovery
- modular deployment

---

# Device Categories

## Sensor Node

A device primarily responsible for telemetry acquisition.

Examples:
- environmental sensor nodes
- water quality monitors
- climate monitoring stations
- soil monitoring probes

Primary responsibilities:
- read sensors
- normalize telemetry
- publish telemetry to MQTT
- report health/status

---

## Actuator Node

A device responsible for controlling physical equipment.

Examples:
- relay controllers
- pump controllers
- fan controllers
- lighting controllers
- valve controllers

Primary responsibilities:
- subscribe to MQTT commands
- execute operations
- report actuator state
- fail safely during faults

---

## Hybrid Node

A node that both:
- reads sensors
- controls actuators

Examples:
- irrigation controller with flow sensing
- hydroponic dosing controller
- nutrient management station

---

## Future Vision Node

Camera or machine-vision device.

Potential capabilities:
- growth analysis
- disease detection
- pest identification
- canopy monitoring
- time-lapse imaging

These systems should remain logically separated from critical automation systems.

---

# Device Identity

Each device must contain:

- unique device ID
- device type
- firmware version
- hardware revision
- capability list
- network identity
- optional location metadata

---

# Device ID Format

Recommended format:

`{location}-{type}-{number}`

Examples:

```
westwall-sensor-1
mainbox-actuator-1
planter4-hybrid-1
reservoir-sensor-1
```

# Device Metadata

Example metadata structure:

```
{
  "deviceId": "westwall-sensor-1",
  "deviceType": "sensor-node",
  "hardwareRevision": "A",
  "firmwareVersion": "1.0.0",
  "manufacturer": "GreenhousePlatform",
  "capabilities": [
    "temperature",
    "humidity",
    "light"
  ],
  "location": {
    "zone": "westwall",
    "description": "West wall planter area"
  }
}
```

# Device Registration

Devices register themselves during startup.

Registration process:

- Boot
- Connect to WiFi
- Connect to MQTT
- Publish registration payload
- Receive configuration
- Enter operational state

# Device Discovery Workflow
## Registration Topic
`greenhouse/discovery/register`

## Example Registration Payload
```
{
  "deviceId": "westwall-sensor-1",
  "deviceType": "sensor-node",
  "firmwareVersion": "1.0.0",
  "hardwareRevision": "A",
  "capabilities": [
    "temperature",
    "humidity",
    "soil-moisture"
  ],
  "ipAddress": "192.168.10.42",
  "macAddress": "AA:BB:CC:DD:EE:FF"
}
```

# Capability-Based Design

The platform should reason about capabilities rather than specific hardware implementations

Examples of capabilities:

- temperature
- humidity
- pump
- relay-output
- pwm-output
- valve
- camera
- co2
- ph
- ec-tds

Advantages:

- interchangeable hardware
- flexible deployments
- easier future expansion
- simpler automation logic

# Device Lifecycle
## Boot Phase

During startup a device:

- initializes hardware
- validates configuration
- connects to WiFi
- synchronizes time if available
- connects to MQTT
- registers itself

## Operational Phase

During normal operation devices:

- publish telemetry
- receive commands
- report heartbeat
- monitor local faults
- support OTA updates

## Fault Phase

If failures occur:

- errors are reported
- safe-state logic activates
- watchdog recovery may occur
- reconnect attempts continue

# Offline Recovery

Devices should tolerate:

- temporary WiFi outages
- MQTT outages
- controller restarts
- power interruptions

without requiring manual intervention.

# Heartbeat Model

Devices publish periodic heartbeat messages.

Heartbeat interval:

- typically 30–60 seconds

Heartbeat payload may include:

- uptime
- RSSI
- free memory
- firmware version
- error state
- sensor fault status
- local temperature

Example:

```
{
  "deviceId": "westwall-sensor-1",
  "uptimeSeconds": 184392,
  "wifiRssi": -58,
  "freeHeap": 183224,
  "firmwareVersion": "1.0.0",
  "healthy": true
}
```

# Firmware Architecture

ESP32 firmware should remain modular.

Recommended layering:

```
Application Layer
    |
Service Layer
    |
Hardware Abstraction Layer
    |
Drivers
    |
Hardware
```

# Firmware Responsibilities
## Application Layer

Responsible for:

- greenhouse logic
- telemetry generation
- actuator decisions
- command routing

## Service Layer

Responsible for:

- MQTT
- WiFi
- OTA updates
- configuration management
- logging

## Hardware Abstraction Layer

Responsible for:

- abstracting physical hardware
- standardizing interfaces
- isolating drivers

## Drivers

Responsible for:

- sensors
- ADC
- relays
- PWM
- I2C
- SPI
- UART

# Configuration Model

Devices should support:

- local default configuration
- remote configuration updates
- persistent configuration storage

Potential storage:

- ESP32 NVS
- SPIFFS/LittleFS

# OTA Update Model

All production devices should support OTA firmware updates.

OTA requirements:

- integrity validation
- rollback support
- staged deployment
- version tracking

# Safety Model

Actuator devices must define:

- safe startup state
- safe failure state
- watchdog behavior

Examples:

- pumps default OFF
- heaters default OFF
- valves default CLOSED

Critical systems should fail safely during:

- reboot
- MQTT loss
- software crash
- watchdog reset

# Local Autonomy

Future devices may support limited local autonomy.

Example:

- local emergency thermal shutdown
- local reservoir overflow protection
- local pump timeout logic

These protections operate independently of the central controller.

# Time Synchronization

Devices should maintain reasonably accurate time.

Preferred approaches:

- NTP from local controller
- Internet NTP if available
- MQTT-provided controller time

Accurate timestamps improve:

- telemetry quality
- historical analysis
- automation debugging

# Logging and Diagnostics

Devices should support:

- serial debugging
- MQTT diagnostics
- firmware version reporting
- structured error reporting

Future enhancements may include:

- remote log streaming
- crash dump reporting
- centralized diagnostics

# Future Expansion

Future device categories may include:

- LoRaWAN gateways
- RS485 industrial bridges
- battery-powered remote sensors
- edge AI processors
- PLC adapters
- solar-powered field nodes

The device model should remain extensible enough to support these without redesigning the architecture.