# Peripheral Onboarding and Reconfiguration User Journey

## Goal

When a peripheral control unit starts and sends heartbeat messages, ensure the Main Control Unit can:

- detect new peripheral units
- detect slot topology changes on known units
- guide the user through onboarding or reconfiguration
- keep Main Control Unit configuration as the single source of truth

This journey is only for peripheral control unit onboarding/reconfiguration.
It does not perform Main Control Unit first-run setup, network setup, or general greenhouse configuration.

## Pre-Conditions/Assumptions

- Main Control Unit setup is complete.
- Main Control Unit receives heartbeat messages on gh/heartbeat.
- Each peripheral control unit reports slot_count and slots details in heartbeat payloads.
- Peripheral units do not persist long-term configuration in Phase 1.

## Routing Rules

Given a heartbeat is received.
When device_id is not known.
Then route to new peripheral onboarding flow.

Given a heartbeat is received.
When device_id is known and discovered slot topology differs from stored configuration.
Then route to peripheral reconfiguration flow.

Given a heartbeat is received.
When device_id is known and discovered slot topology matches stored configuration.
Then continue normal operation and update runtime state only.

## Journey

### New Peripheral Onboarding

Given a new peripheral heartbeat is received.
When no stored configuration exists.
Then the Main Control Unit prompts the user to configure:

- unit name and location
- each discovered slot role (sensor or actuator)
- slot capability mapping
- optional display labels

Given onboarding input is valid.
When user confirms.
Then store peripheral and slot configuration locally and apply configuration to the peripheral.

### Peripheral Reconfiguration

Given a known peripheral heartbeat is received.
When discovered slot topology differs from stored configuration.
Then show a clear prompt that hardware has changed and requires reconfiguration.

Given user confirms reconfiguration.
When updated mapping is saved.
Then store updated configuration, publish updated configuration to peripheral, and resume normal operation.

## Success Criteria

- New peripheral units are detected from heartbeat without manual registration.
- Slot changes on known peripheral units are detected reliably.
- Main Control Unit remains configuration source of truth.
- User can complete onboarding or reconfiguration from the local UI.
- Normal operation resumes after successful configuration.

## Deferred Items

- Automatic capability inference from sensor family ranges beyond initial defaults.
- Bulk onboarding for multiple peripherals at once.
- Historical diff view for prior peripheral configurations.
