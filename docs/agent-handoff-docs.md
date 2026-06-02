# Documentation Agent Handoff

## Current Objective

- Keep documentation internally consistent and implementation-ready for Phase 1.
- Preserve clear separation between Main Control Unit setup flows and Peripheral Control Unit onboarding/reconfiguration flows.
- Keep message contracts deterministic for downstream code-generation agents.

## Scope

- Docs-only updates in this repository.
- No application code changes in this pass.
- Focus areas: architecture, device model, MQTT contracts, and user journeys.

## Relevant Source Documents

- docs/architecture.md
- docs/vision.md
- docs/control-unit-model.md
- docs/device-model.md
- docs/mqtt-topics.md
- docs/journeys/README.md
- docs/journeys/01-Main Unit Setup.md
- docs/journeys/03-Empty Dashboard.md
- docs/journeys/04-Peripheral Onboarding and Reconfiguration.md
- docs/skills/documentation.md

## Decisions Already Made

- Phase 1 is non-containerized.
- Canonical UI project naming uses Greenhouse.UI and Greenhouse.UI.Tests.
- Main Control Unit is the source of truth for all configuration.
- Peripheral units do not persist long-term configuration in Phase 1.
- MQTT JSON payload naming is snake_case.
- Message contracts use unified schemas:
	- Commands share one schema with required id, slot_id, state, value.
	- Responses share one schema with required id, device_id, slot_id, value, state, error_code.
- Heartbeat includes slot_count and slots state/topology details.
- Main Control Unit setup is distinct from Peripheral onboarding/reconfiguration.
- Setup field limits:
	- Greenhouse name max length 50
	- Greenhouse location max length 50
	- Description max length 100
- Phase 1 minimum stable error codes are defined in docs/mqtt-topics.md.

## Open Questions

- Keep required value/state with defaults, or switch to optional value for read commands?
- Should i2c_address remain a hex string (for readability) or be normalized to integer in persisted models?
- Should initial capability inference from I2C ranges be implemented in Phase 1 or deferred?

## Risks / Gaps

- Some documents still carry older language that may need one more consistency pass after code starts.
- New protocol constraints (required fields and defaults) should be mirrored in code validators and tests to prevent drift.
- Peripheral onboarding UI/flow is documented but not yet implemented in code.

## Required Output

- Documentation set that is consistent, unambiguous, and executable by coding agents.
- Explicit flow separation between:
	- Main Control Unit Setup Flow
	- Peripheral Control Unit Onboarding/Reconfiguration Flow
- Stable message contracts with required fields, types, and examples.

## Definition of Done

- All in-scope docs align with the decisions listed above.
- No contradictory setup/onboarding terminology remains in updated docs.
- MQTT payload examples are valid JSON and match declared conventions.
- Handoff contains enough context for another agent to resume without re-discovery.

## Suggested Next Steps

1. Run one final documentation consistency sweep after any upcoming code changes.
2. Implement code-side contract validation/tests against docs/mqtt-topics.md and docs/control-unit-model.md.
3. Decide whether command value should remain required-with-default or become optional, then update docs and contracts in one pass.

## Current Repo State Snapshot

- Branch: dev
- Working tree: documentation changes pending commit
- Modified:
	- docs/architecture.md
	- docs/control-unit-model.md
	- docs/device-model.md
	- docs/journeys/01-Main Unit Setup.md
	- docs/journeys/03-Empty Dashboard.md
	- docs/journeys/README.md
	- docs/mqtt-topics.md
	- docs/vision.md
- Untracked:
	- docs/agent-handoff-docs.md
	- docs/journeys/04-Peripheral Onboarding and Reconfiguration.md
	- docs/skills/
