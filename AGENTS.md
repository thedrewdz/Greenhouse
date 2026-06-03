# AGENTS

## Purpose

Agent operating policy for this repository.

This repository contains the Greenhouse Main Unit C#/.NET application.

This is not the ESP32 Edge Unit firmware repository.

Repository ownership boundaries and operator commands are documented in `README.md`.

## Documentation Source Of Truth

Before taking any other action in this repository, agents must read the central documentation entry point:

- https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md

The Greenhouse Documentation repository is the source of truth for durable project documentation, terminology, architecture, MQTT contracts, ADRs, journeys, skills, development guidance, and quality gates.

Do not recreate those instructions locally. If this file appears to conflict with the Greenhouse Documentation repository, follow the Greenhouse Documentation repository and update this file to remove the conflict.

## Agent Startup Order

1. Read central entry point: https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md
2. Follow central recommended read order:
	- AGENTS.md
	- CONTEXT.md
	- architecture.md
	- device-model.md
	- mqtt-topics.md
	- vision.md
3. Read local `README.md` for repository scope, commands, and central spec/skill links.
4. Read local supplemental docs index: `docs/README.md`.
5. Read `AGENT-HANDOFF.md` before substantial changes.

## Local Supplemental Documentation

Local documents under `docs/` are supplemental implementation guidance for this repository (Main Unit C#/.NET scope).

- Start with the central documentation repository first.
- Use local `docs/` content only as additional repository-scoped guidance.
- If any local document conflicts with the central documentation repository, the central repository wins.

Entry point for local supplemental docs:

- `docs/README.md`

## Working Rules

- Use this repository only for Main Unit application code, tests, project files, and local operational assets required by the application.
- Do not add local copies of durable documentation from the Greenhouse Documentation repository.
- Keep root files limited to repository identity and local operational details not already covered by the central docs repository.
- If durable guidance is missing or stale, update the Greenhouse Documentation repository instead of adding replacement guidance here.

## Session Closeout (Definition Of Done)

Before ending a substantial implementation session:

1. Confirm changes remain inside Main Unit repository scope.
2. Re-check central docs/specs/skills for conflicts.
3. Run canonical verification commands:
	- `dotnet restore Greenhouse.slnx`
	- `dotnet build Greenhouse.slnx --no-restore`
	- `dotnet test Greenhouse.slnx --no-build --no-restore`
4. Confirm tests for changed behavior exist and pass.
5. Update `AGENT-HANDOFF.md` with factual current-session state.
6. Ensure durable guidance changes are proposed in central docs, not duplicated locally.

## Handoff Procedure

Use `AGENT-HANDOFF.md` for cross-session continuity.

At session end, update it with:

- Current objective
- Repository state (branch and notable pending changes)
- Decisions made
- Open questions
- Risks/follow-ups
- Suggested next steps
