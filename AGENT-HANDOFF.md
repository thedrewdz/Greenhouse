# Agent Handoff

## Purpose

This file captures current-session state for agents working in this repository.

It is not durable project documentation. Durable documentation, architecture, contracts, journeys, ADRs, skills, and development guidance belong in the Greenhouse Documentation repository:

- https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md

## Update Rule

Update this file at the end of a substantial session when another agent would need local context to resume efficiently.

Keep entries factual, brief, and tied to the local repository state.

## Current Objective

- Keep Main Unit implementation aligned with central documentation while maintaining repository-scoped supplemental guidance.
- Current local change set adds MQTT .NET contract integration guidance under local skills and indexes it for agent discovery.

## Current Repository State

- Branch: `dev`.
- Working tree before commit: docs skill indexing update pending (`docs/README.md`, `docs/skills/mqtt-contract-integration-dotnet.md`, and this handoff file).
- Last pushed commit before this handoff update: `ee46b20 Add Edge Unit BLE discovery action`.

## Recent Work

- Added Edge Unit discovery contracts under `Greenhouse.Core/Onboarding`.
- Added a dashboard `+` action that scans for advertised `GH-Edge-*` BLE devices.
- Replaced visible UI references to Peripheral with Edge and Main Control Unit with Main Unit.
- Reviewed the repository against central docs/skills and identified contract drift areas.
- Refactored `BlueZEdgeUnitDiscoveryService` out of `Greenhouse.UI` into new `Greenhouse.Bluetooth`.
- Added `Greenhouse.Bluetooth.Tests` for BlueZ parser coverage.
- Added a small `Greenhouse.UI.Tests` smoke test so the UI test project is not empty.
- Verified with `dotnet test Greenhouse.slnx --no-restore`.
- Added local skill guidance `docs/skills/mqtt-contract-integration-dotnet.md` for MQTT publish/subscribe/routing contract integration in .NET.
- Updated `docs/README.md` skills index to include MQTT contract integration guidance so agents can discover and apply it.

## Decisions Made This Session

- Core owns `IEdgeUnitDiscoveryService` and discovery result models.
- `Greenhouse.Bluetooth` owns Linux BlueZ / `bluetoothctl` implementation details.
- `Greenhouse.UI` composes Bluetooth discovery through `AddBluetoothDiscovery()` and should not own BlueZ-specific code.
- The first BLE scan slice filters by advertised name prefix `GH-Edge-*`; a 128-bit provisioning service UUID should be standardized later for stronger filtering.

## Open Questions

- What 128-bit BLE service UUID should Edge Unit provisioning advertise?
- Should Bluetooth scanning eventually move from `bluetoothctl` process control to direct BlueZ D-Bus integration?
- Should local Windows development use a fake Edge Unit discovery implementation?

## Risks Or Follow-Ups

- MQTT command/response/heartbeat models still drift from `mqtt-topics.md` and need contract updates.
- Setup validation limits still need alignment with central docs: greenhouse name 50, location 50, description 100.
- MQTT connection handling still lacks reconnect/resubscribe behavior.
- `Home.razor` still owns too much dashboard/onboarding orchestration and should call an application service.
- `JsonMainConfigRepository` remains a Phase 1 stand-in; central docs prefer SQLite for Phase 1.

## Suggested Next Steps

1. Commit and push the Bluetooth infrastructure refactor.
2. Pull/test on the Raspberry Pi and verify `bluetoothctl` scanning works under the app user.
3. Fix MQTT payload contract drift and add contract tests.
4. Align setup validation limits with central documentation.
5. Move dashboard setup/onboarding orchestration behind application services.
