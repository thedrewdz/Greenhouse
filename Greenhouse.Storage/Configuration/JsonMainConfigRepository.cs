using System.Text.Json;
using Greenhouse.Core.Configuration;
using Greenhouse.Core.Setup.Abstractions;

namespace Greenhouse.Storage.Configuration;

public sealed class JsonMainConfigRepository(string filePath) : IMainConfigRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public async Task<MainConfig?> GetAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<MainConfig>(stream, JsonOptions, cancellationToken);
    }

    public async Task SaveAsync(MainConfig config, CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, config, JsonOptions, cancellationToken);
    }
}
