using Greenhouse.Core.Messaging.Messages;
using Greenhouse.Core.Onboarding;
using Greenhouse.Core.Onboarding.Abstractions;

namespace Greenhouse.Core.Tests;

public sealed class EdgeUnitConfigurationApplicationServiceTests
{
    [Fact]
    public async Task ProcessHeartbeatAsync_CreatesPendingMapping_WhenDeviceIsNew()
    {
        var repository = new InMemoryEdgeUnitConfigurationRepository();
        var service = CreateService(repository);

        var configuration = await service.ProcessHeartbeatAsync(CreateHeartbeat("1ADD5912AF61"));

        Assert.Equal("1ADD5912AF61", configuration.DeviceId);
        Assert.Equal(EdgeUnitConfigurationStatus.PendingRuntimeMapping, configuration.Status);
        Assert.NotNull(configuration.LastHeartbeat);
        Assert.NotNull(await repository.GetByDeviceIdAsync("1ADD5912AF61"));
    }

    [Fact]
    public async Task ProcessHeartbeatAsync_MarksReconfigurationRequired_WhenKnownTopologyChanges()
    {
        var repository = new InMemoryEdgeUnitConfigurationRepository();
        var service = CreateService(repository);
        await repository.SaveAsync(new EdgeUnitConfiguration
        {
            DeviceId = "1ADD5912AF61",
            Name = "Irrigation",
            Status = EdgeUnitConfigurationStatus.Configured,
            SlotMappings =
            [
                new EdgeUnitSlotMapping
                {
                    SlotId = 0,
                    Direction = "sensor",
                    I2cAddress = "0x25",
                    Capability = "moisture"
                }
            ],
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        });

        var configuration = await service.ProcessHeartbeatAsync(
            CreateHeartbeat(
                "1ADD5912AF61",
                new HeartbeatSlotMessage
                {
                    SlotId = 0,
                    Direction = "sensor",
                    I2cAddress = "0x26",
                    Capability = "moisture",
                    State = "none",
                    Value = 42,
                    ErrorCode = 0
                }));

        Assert.Equal(EdgeUnitConfigurationStatus.ReconfigurationRequired, configuration.Status);
    }

    [Fact]
    public async Task SaveRuntimeMappingAsync_StoresMappingAndPublishesConfiguration()
    {
        var repository = new InMemoryEdgeUnitConfigurationRepository();
        var publisher = new CapturingConfigurationPublisher();
        var service = CreateService(repository, publisher);
        await service.ProcessHeartbeatAsync(CreateHeartbeat("1ADD5912AF61"));

        var result = await service.SaveRuntimeMappingAsync(new EdgeUnitMappingRequest(
            "1ADD5912AF61",
            " Bed One ",
            " North Wall ",
            [
                new EdgeUnitSlotMappingRequest(0, "sensor", "0x25", "moisture", " Soil ")
            ]));

        var configuration = await repository.GetByDeviceIdAsync("1ADD5912AF61");
        Assert.True(result.IsValid);
        Assert.NotNull(configuration);
        Assert.Equal(EdgeUnitConfigurationStatus.Configured, configuration.Status);
        Assert.Equal("Bed One", configuration.Name);
        Assert.Equal("North Wall", configuration.Location);
        Assert.Equal("Soil", configuration.SlotMappings[0].DisplayLabel);
        Assert.Equal(configuration, publisher.PublishedConfiguration);
    }

    [Fact]
    public void ValidateProvisioningRequest_RejectsInvalidBrokerUri()
    {
        var service = CreateService(new InMemoryEdgeUnitConfigurationRepository());

        var result = service.ValidateProvisioningRequest(new EdgeUnitProvisioningRequest(
            new DiscoveredEdgeUnit("AA:BB:CC:DD:EE:01", "GH-Edge-1ADD5912AF61", -50),
            "1ADD5912AF61",
            "greenhouse",
            "password",
            new Uri("/relative", UriKind.Relative),
            30000));

        Assert.False(result.IsValid);
        Assert.Contains("MQTT broker URI must be an absolute URI.", result.Errors);
    }

    private static EdgeUnitConfigurationApplicationService CreateService(
        InMemoryEdgeUnitConfigurationRepository repository,
        IEdgeUnitConfigurationPublisher? publisher = null)
    {
        return new EdgeUnitConfigurationApplicationService(
            new StubDiscoveryService(),
            new SuccessfulProvisioningService(),
            repository,
            publisher ?? new CapturingConfigurationPublisher());
    }

    private static HeartbeatMessage CreateHeartbeat(
        string deviceId,
        params HeartbeatSlotMessage[] slots)
    {
        return new HeartbeatMessage
        {
            Id = 1,
            DeviceId = deviceId,
            HardwareRevision = "A",
            FirmwareVersion = "1.0.0",
            UptimeSeconds = 10,
            WifiRssi = -55,
            SlotCount = slots.Length,
            Slots = slots,
            Capabilities = slots.Select(slot => slot.Capability).Distinct().ToArray()
        };
    }

    private sealed class InMemoryEdgeUnitConfigurationRepository : IEdgeUnitConfigurationRepository
    {
        private readonly Dictionary<string, EdgeUnitConfiguration> configurations = new(StringComparer.OrdinalIgnoreCase);

        public Task<IReadOnlyList<EdgeUnitConfiguration>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<EdgeUnitConfiguration>>(configurations.Values.ToArray());
        }

        public Task<EdgeUnitConfiguration?> GetByDeviceIdAsync(
            string deviceId,
            CancellationToken cancellationToken = default)
        {
            configurations.TryGetValue(deviceId, out var configuration);
            return Task.FromResult(configuration);
        }

        public Task SaveAsync(
            EdgeUnitConfiguration configuration,
            CancellationToken cancellationToken = default)
        {
            configurations[configuration.DeviceId] = configuration;
            return Task.CompletedTask;
        }
    }

    private sealed class StubDiscoveryService : IEdgeUnitDiscoveryService
    {
        public Task<EdgeUnitScanResult> ScanAsync(
            TimeSpan duration,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(EdgeUnitScanResult.Success(Array.Empty<DiscoveredEdgeUnit>()));
        }
    }

    private sealed class SuccessfulProvisioningService : IEdgeUnitProvisioningService
    {
        public Task<EdgeUnitProvisioningResult> ProvisionAsync(
            DiscoveredEdgeUnit edgeUnit,
            EdgeUnitProvisioningPayload payload,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(EdgeUnitProvisioningResult.Success());
        }
    }

    private sealed class CapturingConfigurationPublisher : IEdgeUnitConfigurationPublisher
    {
        public EdgeUnitConfiguration? PublishedConfiguration { get; private set; }

        public Task PublishAsync(
            EdgeUnitConfiguration configuration,
            CancellationToken cancellationToken = default)
        {
            PublishedConfiguration = configuration;
            return Task.CompletedTask;
        }
    }
}
