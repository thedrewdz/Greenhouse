# Agent Handoff

## Purpose

This file captures current-session state for agents working in this repository.

It is not durable project documentation. Durable documentation, architecture, contracts, journeys, ADRs, skills, and development guidance belong in the Greenhouse Documentation repository:

- https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md

## Update Rule

Update this file at the end of a substantial session when another agent would need local context to resume efficiently.

Keep entries factual, brief, and tied to the local repository state.

## Current Objective

- Implement the central `edge-unit-configuration` spec in the Main Unit repository.
- Preserve Main Unit boundaries and capture missing central contract details as local spec feedback.

## Current Repository State

- Branch: `main`.
- Working tree has implementation changes for Edge Unit configuration/onboarding orchestration.
- No local CI workflow file exists under `.github/workflows/`.

## Recent Work

- Added Core Edge Unit configuration/onboarding models, provisioning payload/result models, repository/transport abstractions, and `EdgeUnitConfigurationApplicationService`.
- Extended heartbeat messages with `slot_count` and slot topology.
- Added JSON storage for Edge Unit configuration state.
- Replaced the MQTT logging-only router with heartbeat routing into the Edge Unit configuration workflow.
- Added Add Edge Unit and Configure Edge Unit Blazor routes.
- Added focused Core and Storage tests for heartbeat processing, topology drift, runtime mapping, and persistence.
- Added `.agent-output/specs/edge-unit-configuration/implementation-plan.md` and `doc-feedback.md`.

## Decisions Made This Session

- Keep durable policy/spec guidance centralized in Greenhouse-Documentation; do not duplicate locally.
- Do not invent a concrete BLE provisioning GATT contract; use a Core provisioning abstraction and a safe unavailable Bluetooth implementation until central docs define the transport.
- Do not invent a canonical MQTT runtime configuration topic/payload; store mapping locally and invoke a publisher abstraction whose current MQTT implementation logs the missing contract.
- Treat first heartbeat for an unknown Edge Unit as `PendingRuntimeMapping`; mark configured units as `ReconfigurationRequired` when slot direction, I2C address, capability, or slot count differs.

## Open Questions

- What exact BLE GATT service/characteristic/write/response contract should the Main Unit use to deliver the provisioning payload?
- What canonical MQTT topic and payload should publish runtime Edge Unit configuration after mapping or reconfiguration?
- Should `heartbeat_interval_ms` remain editable in the onboarding UI for Phase 1?

## Risks Or Follow-Ups

- Actual BLE payload delivery is not complete until the low-level BLE transport contract is specified and implemented.
- Runtime configuration publish is logged but not transmitted over MQTT until the canonical topic/payload contract exists.
- Existing MQTT command publisher tests still reflect the older minimal command payload shape; command contract alignment remains separate follow-up work.
- No CI workflow currently enforces the documented canonical verification commands.

## Suggested Next Steps

1. Update central docs with BLE provisioning GATT details and MQTT runtime configuration contract.
2. Implement the concrete Bluetooth provisioning transport behind `IEdgeUnitProvisioningService`.
3. Replace `LoggingEdgeUnitConfigurationPublisher` with a contract-compliant MQTT publisher.
4. Add UI/test coverage for successful provisioning once the BLE transport can be exercised.
5. Add a minimal CI workflow that runs the canonical restore/build/test sequence.
