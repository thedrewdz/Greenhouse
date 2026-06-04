# Documentation Feedback: edge-unit-configuration

## Missing Contract Details

- The Main Unit spec requires sending a BLE provisioning payload, but the central docs do not define the concrete BLE GATT service UUID, characteristic UUID, write mode, response characteristic, or pairing/security expectations. Implementation therefore adds a Core provisioning abstraction and a safe unavailable Bluetooth implementation rather than inventing a wire contract.
- The spec requires publishing runtime configuration to the Edge Unit after mapping/reconfiguration, but `mqtt-topics.md` does not define a canonical configuration topic or payload schema. Implementation stores mappings and invokes a publisher abstraction; the current MQTT implementation logs that publishing is skipped until the canonical topic/payload contract exists.

## Spec Quality Notes

- `specs/edge-unit-configuration/spec.md` appears to contain stray transcript text in the middle of the first-time onboarding section.
- `skills/implementation-agent-skill.md` references spec status transitions, but the edge-unit-configuration spec does not expose a visible Spec Control status block.

## Suggested Central Doc Updates

- Add BLE provisioning transport details or link to the exact low-level BLE contract.
- Add MQTT runtime configuration topic and JSON payload schema, including retained-message guidance and failure/ack behavior.
- Remove stray transcript lines from the spec.
- Add explicit Spec Control metadata if implementation-agent status gates remain required.
