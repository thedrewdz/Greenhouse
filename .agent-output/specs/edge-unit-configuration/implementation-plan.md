# Implementation Plan: edge-unit-configuration

## Objective

Implement Main Unit support for Edge Unit onboarding/reconfiguration orchestration from the central `edge-unit-configuration` spec.

## Layer Mapping

- Core: onboarding/configuration application service, runtime mapping models, provisioning payload/response models, heartbeat topology processing, repository and transport abstractions.
- Storage: JSON repository for persisted Edge Unit configuration state.
- MQTT: heartbeat router that deserializes `gh/heartbeat` and forwards valid payloads to Core; placeholder configuration publisher pending canonical MQTT contract.
- Bluetooth: provisioning service abstraction registered with a safe unavailable implementation pending canonical BLE GATT transport details.
- UI: Add Edge Unit scan/provisioning view, dashboard state summary, runtime mapping/reconfiguration view.
- Tests: Core workflow tests and Storage persistence tests.

## Implemented Behavior

- BLE scanning remains cancellable through the existing discovery service.
- Provisioning request validation enforces device ID, Wi-Fi SSID, absolute MQTT broker URI, and bounded optional heartbeat interval.
- Successful provisioning transport response creates a 60-second active onboarding session for first-heartbeat completion.
- First heartbeat for an unknown Edge Unit persists current state and marks the unit `PendingRuntimeMapping`.
- Known configured units with changed slot topology or module identity are marked `ReconfigurationRequired`.
- Runtime mapping stores unit name, location, slot direction, I2C address, capability, and display labels.
- Runtime mapping save invokes a configuration publisher abstraction.

## Verification

- `dotnet build Greenhouse.slnx`
- `dotnet build Greenhouse.slnx --no-restore`
- `dotnet test Greenhouse.slnx --no-build --no-restore`
