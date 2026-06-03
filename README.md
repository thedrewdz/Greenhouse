# Greenhouse Main Unit

C#/.NET Main Unit application for the Greenhouse platform.

Before working in this repository, read the central documentation entry point:

- [Greenhouse Documentation README](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md)

All durable project documentation and development guidance lives in the dedicated Greenhouse Documentation repository.

## Quick Start

```bash
dotnet restore Greenhouse.slnx
dotnet build Greenhouse.slnx --no-restore
dotnet test Greenhouse.slnx --no-build --no-restore
```

## Repository Responsibility

This repository owns Main Unit implementation work only, including:

- Main Unit C#/.NET application code.
- Main Unit tests.
- Main Unit project/solution files.
- Repository-scoped operational files needed to build, run, and test this codebase.
- Local supplemental implementation guidance under `docs/`.

## What This Repository Does Not Own

This repository does not own:

- ESP32 Edge Unit firmware implementation.
- Durable architecture, contract, terminology, journey, ADR, and role guidance.
- Replacement copies of canonical documentation from the Greenhouse Documentation repository.

If durable guidance is missing or stale, update it in the central docs repository instead of adding replacement guidance here.

## Build Command

```bash
dotnet build Greenhouse.slnx
```

## Test Command

```bash
dotnet test Greenhouse.slnx --no-restore
```

## CI Status And Canonical CI Command

This repository currently has no committed workflow under `.github/workflows/`.

Use the following canonical command sequence for local verification and future CI setup:

```bash
dotnet restore Greenhouse.slnx
dotnet build Greenhouse.slnx --no-restore
dotnet test Greenhouse.slnx --no-build --no-restore
```

## Agent Workflow

This file is the repository/operator entry point.

For agent operating policy (startup order, session closeout checklist, handoff procedure), use `AGENTS.md`.

For cross-session state, use `AGENT-HANDOFF.md`.

## Relevant Central Specs

Start from the central specs index:

- [specs/README.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/specs/README.md)

Frequently relevant Main Unit and integration specs:

- [specs/main-unit-setup/spec.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/specs/main-unit-setup/spec.md)
- [specs/network-recovery/spec.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/specs/network-recovery/spec.md)
- [specs/empty-dashboard/spec.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/specs/empty-dashboard/spec.md)
- [specs/edge-unit-configuration/spec.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/specs/edge-unit-configuration/spec.md)
- [specs/edge-unit-onboarding/spec.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/specs/edge-unit-onboarding/spec.md)
- [spec-edge-unit-onboarding-ble.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/spec-edge-unit-onboarding-ble.md)
- [mqtt-topics.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/mqtt-topics.md)

## Relevant Central Skills

Central skill index:

- [skills/README.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/README.md)

Commonly used for this repository:

- [skills/dotnet-clean-architecture.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/dotnet-clean-architecture.md)
- [skills/dotnet-di-without-service-locator.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/dotnet-di-without-service-locator.md)
- [skills/mqtt-contract-integration-dotnet.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/mqtt-contract-integration-dotnet.md)
- [skills/dotnet-storage-and-persistence.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/dotnet-storage-and-persistence.md)
- [skills/dotnet-testing-strategy.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/dotnet-testing-strategy.md)
- [skills/blazor-ui-backend-patterns.md](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/skills/blazor-ui-backend-patterns.md)

## Outputs

Running build and test commands in this repository should produce:

- .NET build output under project `bin/` and `obj/` directories.
- Test results in standard .NET test output streams (and optional TRX output if configured by command options).
- No new durable documentation artifacts in this repository root for platform-wide guidance.

## Local Supplemental Docs

This repository includes local supplemental docs for Main Unit .NET implementation details.

- Start with the central documentation repository.
- Use local docs as additional guidance only.
- If guidance conflicts, the central documentation repository is authoritative.

See local supplemental index: `docs/README.md`.

## Navigation

- Local guardrails: `AGENTS.md`
- Session continuity: `AGENT-HANDOFF.md`
- Local supplemental guidance: `docs/README.md`
- Central documentation entry point: [Greenhouse Documentation README](https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md)
