# Agent Handoff

## Purpose

This file captures current-session state for agents working in this repository.

It is not durable project documentation. Durable documentation, architecture, contracts, journeys, ADRs, skills, and development guidance belong in the Greenhouse Documentation repository:

- https://github.com/thedrewdz/Greenhouse-Documentation/blob/main/README.md

## Update Rule

Update this file at the end of a substantial session when another agent would need local context to resume efficiently.

Keep entries factual, brief, and tied to the local repository state.

## Current Objective

- Reduce redundancy between README and AGENTS and establish clearer single-purpose ownership for each file.
- Keep README repository-focused and AGENTS process-focused for agent execution.

## Current Repository State

- Branch: `main`.
- Working tree currently has local documentation edits pending in `README.md` and `AGENTS.md`.
- No local CI workflow file exists under `.github/workflows/`.

## Recent Work

- Expanded `README.md` with repository responsibility and non-ownership sections.
- Added explicit Quick Start and canonical local verification command sequences.
- Added curated links for relevant central specs and central skills.
- Added outputs expectations.
- Added a final navigation block linking local guardrails/handoff/supplemental docs.
- Refactored `README.md` to remove detailed agent procedure duplication.
- Refactored `AGENTS.md` to be the single source for agent startup order, working rules, session closeout checklist, and handoff procedure.

## Decisions Made This Session

- Keep durable policy/spec guidance centralized in Greenhouse-Documentation; do not duplicate locally.
- Treat the README local verification sequence as the canonical command set for both local checks and future CI configuration.
- Keep detailed agent operating guidance in `AGENTS.md`, with `README.md` pointing to it.

## Open Questions

- Should this repository add a minimal GitHub Actions workflow now to enforce the canonical restore/build/test sequence?
- Should command text be defined once (for example in a dedicated local doc section) and referenced from both README and AGENTS to avoid future command drift?

## Risks Or Follow-Ups

- Documentation drift risk remains for command/checklist content that appears in both `README.md` and `AGENTS.md`.
- No CI workflow currently enforces the documented canonical verification commands.
- Existing product-level follow-ups from prior sessions (MQTT contract drift, setup validation limits, reconnect behavior, UI orchestration boundaries, SQLite migration) remain open until explicitly revalidated.

## Suggested Next Steps

1. Commit and push the current documentation updates (`README.md`, `AGENTS.md`, and this handoff file).
2. Add a minimal CI workflow that runs the canonical restore/build/test sequence.
3. Optionally centralize the canonical command sequence in one local source block and reference it from both README and AGENTS.
4. Continue product-level follow-ups from prior sessions: MQTT contract alignment, setup validation limits, reconnect behavior, UI orchestration split, and persistence strategy alignment.
