# Empty Dashboard User Journey

## Goal

When the Main Control Unit is configured but no peripheral units are connected yet, show a useful dashboard state that explains why the system has limited functionality and points the user toward connecting sensor or actuator units.

## Pre-Conditions/Assumptions

- General configuration already exists.
- The Main Control Unit can load the normal dashboard.
- No sensor units, actuator units, or hybrid peripheral units have been registered.
- No automation rules have been created.
- The dashboard should remain useful even before peripherals are connected.

## Journey

**Given** the Main Control Unit has completed setup.
**When** the dashboard loads and no peripheral units are registered.
**Then** show the normal dashboard shell with an empty peripherals section.

The empty peripherals section should:

- Reserve the area where connected sensor and actuator units will appear later.
- Explain that no peripheral units are connected yet.
- Prompt the user to connect a sensor or actuator unit.
- Avoid presenting this as an error condition.

Suggested message:

```text
Connect a sensor or actuator unit to get more from your unit.
```

**Given** the dashboard loads and no automation rules exist.
**When** the dashboard displays the rules section.
**Then** show an empty rules state rather than hiding the section entirely.

The empty rules section should:

- Reserve the area where automation rules will appear later.
- Explain that rules can be created after peripherals are available.
- Avoid asking the user to create rules before any peripheral units exist.

## Success Criteria

- The dashboard does not feel broken or unfinished when no peripheral units are connected.
- The user can see where connected units and rules will appear later.
- The user is prompted to connect a sensor or actuator unit.
- The dashboard avoids actions that cannot be completed until peripherals exist.

## Deferred Journeys

- Peripheral onboarding and reconfiguration (see `04-Peripheral Onboarding and Reconfiguration.md`).
- Creating automation rules.
- Viewing live telemetry from connected units.
