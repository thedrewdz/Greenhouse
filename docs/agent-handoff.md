# Agent Handoff

## Current Goal

Continue Phase 1 implementation of the Greenhouse Automation Platform from the new `dev` working copy.

The immediate foundation now exists:

- first-run setup flow
- network recovery flow
- appliance-oriented dashboard shell
- contract-first messaging abstractions
- MQTTnet-backed messaging infrastructure

Next work should build on these contracts rather than bypassing them.

## Branch / Workspace Transition

The previous working folder was:

```text
D:\Code\Greenhouse\p1-setup
```

The user has pushed and merged the work to `main`, then created a new branch called `dev` and cloned/opened it at:

```text
D:\Code\Greenhouse\dev
```

On resume, use the new folder as the working directory.

First checks:

```powershell
git branch --show-current
git status --short
dotnet test Greenhouse.slnx --no-restore
```

Expected branch:

```text
dev
```

## Recent Commits

The last completed commits in the previous workspace were:

- `c02cf52 Initial project scaffold, documentation, and design`
- `1a6c0fe Implement first-run setup and appliance dashboard`
- `1993b87 Add contract-first messaging infrastructure`

These were pushed/merged by the user before creating the `dev` branch.

## Current Implementation State

### Setup and Dashboard

- `Greenhouse.UI` hosts the Blazor UI.
- `/setup` implements first-run setup.
- `/network` implements manual network reconnection/recovery.
- `/` redirects to setup when general configuration is missing.
- `/` shows a compact appliance dashboard when setup is complete.
- Dashboard is optimized for a small touch screen target: 1024x600 native resolution, physical size around 150mm x 85mm.
- Runtime app data is ignored under `Greenhouse.UI/App_Data/`.

### Configuration

- General configuration model lives in `Greenhouse.Core/Configuration/MainConfig.cs`.
- Configuration persistence contract lives in `Greenhouse.Core/Setup/Abstractions/IMainConfigRepository.cs`.
- Current persistence implementation is JSON-backed in `Greenhouse.Storage/Configuration/JsonMainConfigRepository.cs`.
- JSON storage is a Phase 1 stand-in; SQLite is still expected later.

### Network

- Network abstraction is `INetworkService`.
- The current implementation is `DevelopmentNetworkService` in `Greenhouse.UI/Infrastructure/`.
- It simulates connection success and does not change Wi-Fi on the host.
- Future Raspberry Pi implementation should likely call Debian/NetworkManager tooling such as `nmcli`, but behind `INetworkService`.

### Messaging

Core has no MQTT-specific namespace or contract names.

Core messaging contracts live under:

```text
Greenhouse.Core/Messaging/
```

Important contracts/models:

- `MessagingOptions`
- `MessagingTopics`
- `MessageEnvelope`
- `IMessagingRepository`
- `ICommandPublisher`
- `IMessageRouter`
- `IConnectedService`
- message payload records under `Greenhouse.Core/Messaging/Messages/`

MQTT-specific implementation lives under:

```text
Greenhouse.Mqtt/
```

Important implementation classes:

- `Repository`
- `CommandPublisher`
- `ConnectedService`
- `LoggingMessageRouter`
- `JsonMessageSerializer`
- `ServiceCollectionExtensions`

`Greenhouse.UI` wires messaging with:

```csharp
builder.Services.AddMessaging(builder.Configuration);
```

Configuration key:

```json
"Messaging": {
  "Host": "localhost",
  "Port": 1883,
  "ClientId": "greenhouse-main-control"
}
```

The current `ConnectedService` tries to connect at startup, subscribes to:

- `gh/heartbeat`
- `gh/ack`
- `gh/rd`

If the broker is unavailable, the web app should not crash.

## Current Documentation

Important docs:

- `docs/control-unit-model.md`
- `docs/journeys/01-Main Unit Setup.md`
- `docs/journeys/02-Network Recovery.md`
- `docs/journeys/03-Empty Dashboard.md`
- `docs/mqtt-topics.md`
- `docs/device-model.md`
- `docs/architecture.md`

`docs/control-unit-model.md` now contains explicit coding rules:

- contract-first design
- avoid tight coupling
- avoid tight coupling in naming conventions
- use technology-neutral core contracts
- keep implementation-specific names/details inside the owning implementation project
- avoid redundant names within any project or namespace

These rules apply across the whole solution, not only to MQTT.

## Architectural Decisions To Preserve

- Build contract-first.
- Avoid direct infrastructure references from UI/application/domain code.
- Do not put MQTT-specific names in `Greenhouse.Core`.
- Do not use redundant prefixes inside implementation projects unless needed to remove ambiguity.
- Use `Greenhouse.Mqtt` as an infrastructure implementation of generic messaging contracts.
- Keep UI appliance-focused for a small touch screen.
- Do not overbuild cloud, AI, auth, OTA, or automation rules before the core local workflow works.

## Likely Next Steps

1. Switch to `D:\Code\Greenhouse\dev`.
2. Confirm branch and clean status.
3. Run tests.
4. Decide the next user journey.
5. Likely next implementation area: device heartbeat ingestion.

For heartbeat ingestion, stay contract-first:

- define device registry/heartbeat contracts in `Greenhouse.Core`
- route `gh/heartbeat` through `IMessageRouter`
- store/update known devices behind a repository abstraction
- update dashboard peripheral count from an application service
- keep MQTTnet-specific parsing/transport details inside `Greenhouse.Mqtt`

## Open Questions

- Should Phase 1 persistence move from JSON to SQLite now or after heartbeat ingestion?
- Should device registry live first in memory, then persist later, or go directly to SQLite?
- Should the dashboard display raw heartbeat count, known peripheral count, or device cards first?
- When should the real Raspberry Pi `INetworkService` be implemented?
- Should the Mosquitto broker be installed as an OS service on the Pi, or managed by the app deployment process?

## Resume Prompt

Use this after switching workspaces:

```text
Read docs/agent-handoff.md in D:\Code\Greenhouse\dev and continue from the likely next steps.
```
