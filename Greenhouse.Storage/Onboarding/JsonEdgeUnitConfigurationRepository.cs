using System.Text.Json;
using Greenhouse.Core.Onboarding;
using Greenhouse.Core.Onboarding.Abstractions;

namespace Greenhouse.Storage.Onboarding;

public sealed class JsonEdgeUnitConfigurationRepository(string filePath) : IEdgeUnitConfigurationRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public async Task<IReadOnlyList<EdgeUnitConfiguration>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var store = await ReadStoreAsync(cancellationToken);
        return store.EdgeUnits
            .OrderBy(edgeUnit => edgeUnit.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public async Task<EdgeUnitConfiguration?> GetByDeviceIdAsync(
        string deviceId,
        CancellationToken cancellationToken = default)
    {
        var store = await ReadStoreAsync(cancellationToken);
        return store.EdgeUnits.FirstOrDefault(edgeUnit =>
            string.Equals(edgeUnit.DeviceId, deviceId, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveAsync(
        EdgeUnitConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var store = await ReadStoreAsync(cancellationToken);
        var remaining = store.EdgeUnits
            .Where(edgeUnit => !string.Equals(edgeUnit.DeviceId, configuration.DeviceId, StringComparison.OrdinalIgnoreCase))
            .Append(configuration)
            .OrderBy(edgeUnit => edgeUnit.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, new EdgeUnitConfigurationStore(remaining), JsonOptions, cancellationToken);
    }

    private async Task<EdgeUnitConfigurationStore> ReadStoreAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
        {
            return new EdgeUnitConfigurationStore(Array.Empty<EdgeUnitConfiguration>());
        }

        await using var stream = File.OpenRead(filePath);
        var store = await JsonSerializer.DeserializeAsync<EdgeUnitConfigurationStore>(stream, JsonOptions, cancellationToken);
        return store ?? new EdgeUnitConfigurationStore(Array.Empty<EdgeUnitConfiguration>());
    }

    private sealed record EdgeUnitConfigurationStore(IReadOnlyList<EdgeUnitConfiguration> EdgeUnits);
}
