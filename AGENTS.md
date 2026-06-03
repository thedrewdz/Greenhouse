# AGENTS

## Purpose

This repository contains the Greenhouse Main Unit C#/.NET application.

This is not the ESP32 Edge Unit firmware repository.

## Documentation Source Of Truth

Before taking any other action in this repository, agents must read the central documentation entry point:

- https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md

The Greenhouse Documentation repository is the source of truth for durable project documentation, terminology, architecture, MQTT contracts, ADRs, journeys, skills, development guidance, and quality gates.

Do not recreate those instructions locally. If this file appears to conflict with the Greenhouse Documentation repository, follow the Greenhouse Documentation repository and update this file to remove the conflict.

## Local Supplemental Documentation

Local documents under `docs/` are supplemental implementation guidance for this repository (Main Unit C#/.NET scope).

- Start with the central documentation repository first.
- Use local `docs/` content only as additional repository-scoped guidance.
- If any local document conflicts with the central documentation repository, the central repository wins.

Entry point for local supplemental docs:

- `docs/README.md`

## Local Repository Scope

Use this repository only for Main Unit application code, tests, project files, and local operational assets required by the application.

Do not add local copies of durable documentation from the Greenhouse Documentation repository.

## Local Files

Keep root files in this repository limited to repository identity and local operational details that are not already covered by the Greenhouse Documentation repository.

When durable guidance is missing or stale, update the Greenhouse Documentation repository instead of adding replacement guidance here.
