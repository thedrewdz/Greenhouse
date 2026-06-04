using System.Collections.Concurrent;
using Greenhouse.Core.Messaging.Messages;
using Greenhouse.Core.Onboarding.Abstractions;
using Greenhouse.Core.Setup;

namespace Greenhouse.Core.Onboarding;

public sealed class EdgeUnitConfigurationApplicationService(
    IEdgeUnitDiscoveryService discoveryService,
    IEdgeUnitProvisioningService provisioningService,
    IEdgeUnitConfigurationRepository configurationRepository,
    IEdgeUnitConfigurationPublisher configurationPublisher)
{
    public const int DefaultScanSeconds = 10;
    public const int DefaultOnboardingSessionSeconds = 60;
    public const int MinimumHeartbeatIntervalMs = 5_000;
    public const int MaximumHeartbeatIntervalMs = 300_000;

    private readonly ConcurrentDictionary<string, OnboardingSession> activeSessions = new(StringComparer.OrdinalIgnoreCase);

    public Task<EdgeUnitScanResult> ScanAsync(CancellationToken cancellationToken = default)
    {
        return discoveryService.ScanAsync(TimeSpan.FromSeconds(DefaultScanSeconds), cancellationToken);
    }

    public async Task<EdgeUnitProvisioningResult> ProvisionAsync(
        EdgeUnitProvisioningRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = ValidateProvisioningRequest(request);
        if (!validation.IsValid)
        {
            return EdgeUnitProvisioningResult.Failure(2001, validation.Errors[0]);
        }

        var payload = new EdgeUnitProvisioningPayload
        {
            DeviceId = request.DeviceId.Trim(),
            WifiSsid = request.WifiSsid.Trim(),
            WifiPassword = request.WifiPassword,
            MqttBrokerUri = request.MqttBrokerUri.ToString(),
            HeartbeatIntervalMs = request.HeartbeatIntervalMs
        };

        var result = await provisioningService.ProvisionAsync(request.EdgeUnit, payload, cancellationToken);
        if (!result.Succeeded)
        {
            return result;
        }

        var now = DateTimeOffset.UtcNow;
        activeSessions[payload.DeviceId] = new OnboardingSession(
            payload.DeviceId,
            request.EdgeUnit,
            now,
            now.AddSeconds(DefaultOnboardingSessionSeconds));

        return EdgeUnitProvisioningResult.Success();
    }

    public ValidationResult ValidateProvisioningRequest(EdgeUnitProvisioningRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.DeviceId))
        {
            errors.Add("Device ID is required.");
        }

        if (string.IsNullOrWhiteSpace(request.WifiSsid))
        {
            errors.Add("Wi-Fi SSID is required.");
        }

        if (request.MqttBrokerUri is null || !request.MqttBrokerUri.IsAbsoluteUri)
        {
            errors.Add("MQTT broker URI must be an absolute URI.");
        }

        if (request.HeartbeatIntervalMs is not null
            && (request.HeartbeatIntervalMs < MinimumHeartbeatIntervalMs
                || request.HeartbeatIntervalMs > MaximumHeartbeatIntervalMs))
        {
            errors.Add($"Heartbeat interval must be between {MinimumHeartbeatIntervalMs} and {MaximumHeartbeatIntervalMs} ms.");
        }

        return errors.Count == 0
            ? ValidationResult.Success
            : ValidationResult.Failure(errors);
    }

    public async Task<EdgeUnitConfiguration> ProcessHeartbeatAsync(
        HeartbeatMessage heartbeat,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(heartbeat.DeviceId))
        {
            throw new InvalidOperationException("Heartbeat device_id is required.");
        }

        var now = DateTimeOffset.UtcNow;
        var existing = await configurationRepository.GetByDeviceIdAsync(heartbeat.DeviceId, cancellationToken);
        var isActiveOnboarding = activeSessions.TryGetValue(heartbeat.DeviceId, out var session)
            && session.ExpiresAtUtc >= now;

        if (isActiveOnboarding)
        {
            activeSessions.TryRemove(heartbeat.DeviceId, out _);
        }

        if (existing is null)
        {
            var discoveredName = isActiveOnboarding ? session!.EdgeUnit.Name : heartbeat.DeviceId;
            var configuration = new EdgeUnitConfiguration
            {
                DeviceId = heartbeat.DeviceId,
                Name = discoveredName,
                Status = EdgeUnitConfigurationStatus.PendingRuntimeMapping,
                LastHeartbeat = heartbeat,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            await configurationRepository.SaveAsync(configuration, cancellationToken);
            return configuration;
        }

        var updatedStatus = existing.Status;
        if (existing.Status == EdgeUnitConfigurationStatus.Configured
            && HasTopologyDrift(existing.SlotMappings, heartbeat.Slots))
        {
            updatedStatus = EdgeUnitConfigurationStatus.ReconfigurationRequired;
        }

        var updated = existing with
        {
            LastHeartbeat = heartbeat,
            Status = updatedStatus,
            UpdatedAtUtc = now
        };

        await configurationRepository.SaveAsync(updated, cancellationToken);
        return updated;
    }

    public async Task<ValidationResult> SaveRuntimeMappingAsync(
        EdgeUnitMappingRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = ValidateRuntimeMapping(request);
        if (!validation.IsValid)
        {
            return validation;
        }

        var existing = await configurationRepository.GetByDeviceIdAsync(request.DeviceId, cancellationToken);
        if (existing is null)
        {
            return ValidationResult.Failure("Edge Unit has not published a heartbeat yet.");
        }

        var now = DateTimeOffset.UtcNow;
        var configuration = existing with
        {
            Name = request.Name.Trim(),
            Location = string.IsNullOrWhiteSpace(request.Location) ? null : request.Location.Trim(),
            SlotMappings = request.Slots
                .Select(slot => new EdgeUnitSlotMapping
                {
                    SlotId = slot.SlotId,
                    Direction = slot.Direction.Trim(),
                    I2cAddress = slot.I2cAddress.Trim(),
                    Capability = slot.Capability.Trim(),
                    DisplayLabel = string.IsNullOrWhiteSpace(slot.DisplayLabel) ? null : slot.DisplayLabel.Trim()
                })
                .OrderBy(slot => slot.SlotId)
                .ToArray(),
            Status = EdgeUnitConfigurationStatus.Configured,
            UpdatedAtUtc = now
        };

        await configurationRepository.SaveAsync(configuration, cancellationToken);
        await configurationPublisher.PublishAsync(configuration, cancellationToken);

        return ValidationResult.Success;
    }

    public Task<IReadOnlyList<EdgeUnitConfiguration>> GetConfigurationsAsync(CancellationToken cancellationToken = default)
    {
        return configurationRepository.GetAllAsync(cancellationToken);
    }

    public Task<EdgeUnitConfiguration?> GetConfigurationAsync(
        string deviceId,
        CancellationToken cancellationToken = default)
    {
        return configurationRepository.GetByDeviceIdAsync(deviceId, cancellationToken);
    }

    private static ValidationResult ValidateRuntimeMapping(EdgeUnitMappingRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.DeviceId))
        {
            errors.Add("Device ID is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add("Edge Unit name is required.");
        }

        if (request.Slots.Count == 0)
        {
            errors.Add("At least one slot mapping is required.");
        }

        foreach (var slot in request.Slots)
        {
            if (slot.SlotId < 0)
            {
                errors.Add("Slot IDs must be zero or greater.");
            }

            if (string.IsNullOrWhiteSpace(slot.Direction))
            {
                errors.Add($"Slot {slot.SlotId} direction is required.");
            }

            if (string.IsNullOrWhiteSpace(slot.I2cAddress))
            {
                errors.Add($"Slot {slot.SlotId} I2C address is required.");
            }

            if (string.IsNullOrWhiteSpace(slot.Capability))
            {
                errors.Add($"Slot {slot.SlotId} capability is required.");
            }
        }

        return errors.Count == 0
            ? ValidationResult.Success
            : ValidationResult.Failure(errors);
    }

    private static bool HasTopologyDrift(
        IReadOnlyList<EdgeUnitSlotMapping> storedSlots,
        IReadOnlyList<HeartbeatSlotMessage> heartbeatSlots)
    {
        if (storedSlots.Count != heartbeatSlots.Count)
        {
            return true;
        }

        var storedBySlot = storedSlots.ToDictionary(slot => slot.SlotId);
        foreach (var heartbeatSlot in heartbeatSlots)
        {
            if (!storedBySlot.TryGetValue(heartbeatSlot.SlotId, out var storedSlot))
            {
                return true;
            }

            if (!string.Equals(storedSlot.Direction, heartbeatSlot.Direction, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(storedSlot.I2cAddress, heartbeatSlot.I2cAddress, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(storedSlot.Capability, heartbeatSlot.Capability, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
