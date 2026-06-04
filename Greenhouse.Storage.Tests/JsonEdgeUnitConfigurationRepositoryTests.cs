using Greenhouse.Core.Onboarding;
using Greenhouse.Storage.Onboarding;

namespace Greenhouse.Storage.Tests;

public sealed class JsonEdgeUnitConfigurationRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenStoreDoesNotExist()
    {
        var repository = new JsonEdgeUnitConfigurationRepository(CreateTempFilePath());

        var configurations = await repository.GetAllAsync();

        Assert.Empty(configurations);
    }

    [Fact]
    public async Task SaveAsync_UpsertsConfigurationByDeviceId()
    {
        var repository = new JsonEdgeUnitConfigurationRepository(CreateTempFilePath());
        var initial = CreateConfiguration("1ADD5912AF61", "Original");
        var updated = initial with
        {
            Name = "Updated",
            Status = EdgeUnitConfigurationStatus.Configured,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.SaveAsync(initial);
        await repository.SaveAsync(updated);

        var configurations = await repository.GetAllAsync();
        var actual = await repository.GetByDeviceIdAsync("1ADD5912AF61");

        Assert.Single(configurations);
        Assert.NotNull(actual);
        Assert.Equal("Updated", actual.Name);
        Assert.Equal(EdgeUnitConfigurationStatus.Configured, actual.Status);
    }

    private static EdgeUnitConfiguration CreateConfiguration(string deviceId, string name)
    {
        return new EdgeUnitConfiguration
        {
            DeviceId = deviceId,
            Name = name,
            Status = EdgeUnitConfigurationStatus.PendingRuntimeMapping,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private static string CreateTempFilePath()
    {
        return Path.Combine(Path.GetTempPath(), "greenhouse-tests", Guid.NewGuid().ToString("N"), "edge-units.json");
    }
}
