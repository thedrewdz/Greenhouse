# Agent Handoff

## Current Goal

Continue building the Greenhouse Automation Platform from the Phase 1 documentation and current .NET solution scaffold.

The immediate focus is to turn the control-unit documentation into a working main control unit foundation: domain models, MQTT contracts/services, storage, and a Blazor-based UI.

## Last Known Context

- The previous work centered on analyzing and refining the documentation in `docs/`.
- The docs now describe a local-first greenhouse automation system centered around a main control unit, MQTT messaging, ESP32 peripheral nodes, local persistence, and a web UI.
- The system is intended to stay simple in Phase 1 and avoid overbuilding future concerns such as cloud sync, AI, OTA, and containerization.
- A .NET solution and project structure appear to have been scaffolded after or alongside the documentation work.

## Important Documentation

- `docs/vision.md` describes the product direction and guiding principles.
- `docs/architecture.md` describes the local-first architecture, main control unit, MQTT broker, peripheral nodes, automation engine, storage, and future expansion areas.
- `docs/device-model.md` describes ESP32 peripheral units, device identity, metadata, registration, capabilities, lifecycle, heartbeat, firmware responsibilities, safety, and time model.
- `docs/mqtt-topics.md` describes the simplified MQTT command and response topic design.
- `docs/control-unit-model.md` is the main implementation instruction document for the control unit.
- `docs/journeys/01-Main Unit Setup.md` describes the setup journey for the main control unit.

## Current Architectural Direction

- Main control unit runs locally, likely on a Raspberry Pi.
- Phase 1 deployment should avoid Docker/containerization unless later needed.
- Main unit owns configuration, timing, automation, persistence, and UI.
- Peripheral units are intentionally simple.
- Peripheral device IDs are based on Wi-Fi MAC address.
- Peripheral units do not persist full configuration; they receive configuration from the main unit during startup.
- MQTT is the primary communication boundary between the main unit and peripheral nodes.
- SQLite or LiteDB-style local persistence is expected for Phase 1.
- UI is expected to be a Blazor-based local web app/kiosk interface.

## MQTT Direction

Peripheral subscribe topics:

- `ghcmd/rd-{deviceId}`
- `ghcmd/wr-{deviceId}`

Peripheral publish topics:

- `gh/heartbeat`
- `gh/ack`
- `gh/rd`

Main unit subscribes to:

- `gh/heartbeat`
- `gh/ack`
- `gh/rd`

Main unit publishes to:

- `ghcmd/rd-{deviceId}`
- `ghcmd/wr-{deviceId}`

Payloads are JSON for Phase 1.

## Current Repo State To Check On Resume

Run:

```powershell
git status --short
```

At the time this handoff was created, the repo showed:

- Modified tracked docs:
  - `docs/architecture.md`
  - `docs/device-model.md`
  - `docs/mqtt-topics.md`
  - `docs/vision.md`
- Untracked docs:
  - `docs/control-unit-model.md`
  - `docs/journeys/`
- Untracked solution/project folders:
  - `Greenhouse.Core/`
  - `Greenhouse.Core.Tests/`
  - `Greenhouse.Mqtt/`
  - `Greenhouse.Mqtt.Tests/`
  - `Greenhouse.Storage/`
  - `Greenhouse.Storage.Tests/`
  - `Greenhouse.UI/`
  - `Greenhouse.UI.Tests/`
  - `Greenhouse.slnx`
  - `Design/`

Do not assume these are committed. Check the current state before editing.

## Likely Next Steps

1. Read `docs/control-unit-model.md` first.
2. Inspect the scaffolded .NET projects and solution file.
3. Compare project names against the docs and normalize naming if needed.
4. Identify the first coding target from `docs/control-unit-model.md`.
5. Implement the smallest useful Phase 1 foundation, probably starting with:
   - domain/device models in `Greenhouse.Core`
   - MQTT topic and payload contracts in `Greenhouse.Mqtt`
   - persistence models/repositories in `Greenhouse.Storage`
   - basic UI shell in `Greenhouse.UI`
6. Add focused tests for contracts and model behavior.
7. Run the relevant .NET test/build commands.

## Open Questions

- Should persistence use SQLite directly, Entity Framework Core, Dapper, or LiteDB?
- Should project names match `Greenhouse.ControlUnit.UI` from the docs or the currently scaffolded `Greenhouse.UI`?
- Should MQTT use an external broker installed on the Raspberry Pi, an embedded broker, or only an MQTT client library in the app?
- Should authentication be included in Phase 1 or deferred until after local setup and device registration flows are working?
- What is the first real hardware target: mock MQTT devices, ESP32 firmware, or the Raspberry Pi control unit?

## How To Resume

Start the next session with:

```text
Read docs/agent-handoff.md and continue from the likely next steps.
```

Then inspect the repo before making changes.
