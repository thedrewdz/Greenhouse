# AGENTS

## Purpose

This repository contains the Greenhouse Main Unit application.

This is not the ESP32 Edge Unit firmware repository.

## First Action

Before taking any other action in this repository, agents must read the central documentation entry point:

- https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md

Use the dedicated Greenhouse Documentation repository for durable project documentation, canonical context, architecture, MQTT contracts, ADRs, journeys, and skill guidance.

## Ambiguity Handling

- Replace fuzzy terms with the canonical term from the Greenhouse Documentation repository.
- When a plan, requirement, or architecture direction is ambiguous, apply the relevant documentation, planning, or review guidance from the Greenhouse Documentation repository.
- Explore existing central docs and local code before asking the user a question.
- Ask only the highest-leverage unresolved question, include a recommended default answer, and wait for feedback before asking the next question.
- Resolve terminology conflicts in touched local instructions immediately.

## Scope Boundaries

- Keep work focused on Main Unit C#/.NET UI and backend concerns.
- Do not introduce Edge Unit firmware implementation details into this repository.
- Keep architecture local-first and MQTT-centered.
- Do not introduce cloud-first assumptions into Phase 1 control paths.
- Keep durable documentation in the Greenhouse Documentation repository, not in local copies.

## Instruction Precedence

Use this precedence order when instructions overlap:

1. AGENTS.md (this file)
2. .github/copilot-instructions.md
3. Greenhouse Documentation repository instructions and docs

If guidance conflicts, follow the highest-precedence source.

## Coding Standards

- Use contract-first design.
- Keep `Greenhouse.Core` technology-neutral.
- Keep UI concerns in `Greenhouse.UI`.
- Keep MQTT transport details in `Greenhouse.Mqtt`.
- Keep storage details in `Greenhouse.Storage`.
- Depend on abstractions from application and domain code.
- Use explicit constructor injection and avoid service locator patterns.
- Keep Blazor components focused on rendering, input capture, and user feedback.
- Put workflow orchestration in application services.
- Preserve explicit `Program.Main` style instead of top-level statements.

## Runtime Rules

- Preserve local-first behavior for critical greenhouse control.
- Treat MQTT broker, WiFi, malformed payload, and storage failures as recoverable conditions.
- Keep UI and backend behavior usable when the Main Unit is offline.
- Preserve canonical MQTT topic and payload contracts from the Greenhouse Documentation repository.
- Do not let UI components publish MQTT messages directly.
- Do not store WiFi credentials in the application database.
- Keep actuator commands safety-sensitive and auditable.

## Quality Gates

Before finalizing changes, verify:

1. Changes remain aligned with the canonical context, architecture, journeys, ADRs, and MQTT contracts in the Greenhouse Documentation repository.
2. Class and service responsibilities are single-purpose.
3. Core abstractions do not depend on UI, storage, MQTT, ASP.NET Core, or other infrastructure concerns.
4. Dependency injection is explicit and does not use service locator patterns.
5. UI changes remain touch-friendly for the Main Unit appliance experience.
6. Offline, reconnect, malformed payload, and negative-path behavior is handled deterministically.
7. Tests are added or updated at the correct layer for changed behavior.
