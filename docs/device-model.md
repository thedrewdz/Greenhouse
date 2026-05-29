# Device Model

# Overview

The greenhouse platform uses a distributed device model built around modular ESP32-based nodes.
Each node is responsible for attached peripherals, but is essentially dumb.

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

A device responsible for controlling physical equipment (through relays)

Examples:
- pumps
- fans
- lighting
- solanoid valves

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

Camera or machine-vision device. Most probably just streaming frames from a web cam to be processed online.

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

- unique device ID -WIFI MAC address
- firmware version
- hardware revision
- capability list - analogous with connected peripherals
- general fault id - 0 - none
- fault list - fault states per capability/peripheral

---

# Device Metadata

Example metadata structure:

```
{
  "deviceId": "1ADD5912AF61",
  "hardwareRevision": "A",
  "firmwareVersion": "1.0.0",
  "uptimeSeconds": 92384,
  "wifiRssi": -61,
  "manufacturer": "GreenhousePlatform",
  "capabilities": [
    "temperature",
    "humidity",
    "light"
  ]
}
```

# Device Registration

- Devices register themselves during startup
- Devices do not store config
- Config is always received from the main unit during startup whether first time or subsequent. 
- From the device perspective, registration and startup is the same.

Registration process:

- Boot
- Connect to WiFi
- Connect to MQTT
- Publish heartbeat containing device metadata
- Receive configuration from main unit via MQTT
- Enter operational state

# Capability-Based Design

The platform should reason about capabilities and their location where possible rather than specific hardware implementations

Examples of capabilities:

- temperature
- humidity
- pump
- valve
- camera
- co2
- ph
- ec-tds
- light
- time of day
- season

Advantages:

- interchangeable hardware
- flexible deployments
- easier future expansion
- simpler automation logic

# Device Lifecycle
## Boot Phase

During startup a device:

- Boot
- Connect to WiFi
- Connect to MQTT
- Publish heartbeat containing device metadata
- Receive configuration from main unit via MQTT
- Enter operational state

## Operational Phase

During normal operation devices:

- receive commands
- publish telemetry
- report heartbeat
- monitor local faults
- [future] support OTA updates

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
- [future] temporary loss of the main unit

without requiring manual intervention.

# Heartbeat Model

Devices publish periodic heartbeat messages.

Heartbeat interval:

- typically 30–60 seconds

Heartbeat payload should be the device metadata, and could include any curent fault states (possibly general and per capability).

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

- telemetry generation
- actuator activation/deactivation
- actuator safeguards by type
- command routing

## Service Layer

Responsible for:

- MQTT
- WiFi
- heartbeat
- [future] OTA updates

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

# Future OTA Update Model

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
- local scheduling if the central control unit becomes unavailable

These protections operate independently of the central controller.

# Time Synchronization

To avoid over developing, we will not attempt to maintain time on all devices.

- Central control unit (main unit) is responsible for timing
- Peripheral units maintain a integer message index that is incremented with every payload published
- message index can be assigned during startup routine based on stored value from main unit.

