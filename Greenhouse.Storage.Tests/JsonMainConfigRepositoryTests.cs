using Greenhouse.Core.Configuration;
using Greenhouse.Storage.Configuration;

namespace Greenhouse.Storage.Tests;

public sealed class JsonMainConfigRepositoryTests
{
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenConfigFileDoesNotExist()
    {
        var repository = new JsonMainConfigRepository(CreateTempFilePath());

        var config = await repository.GetAsync();

        Assert.Null(config);
    }

    [Fact]
    public async Task SaveAsync_WritesConfigurationThatCanBeReadBack()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonMainConfigRepository(filePath);
        var expected = new MainConfig
        {
            GreenhouseName = "Vegetable Garden",
            GreenhouseLocation = "Back Garden",
            Description = "Hydroponics",
            CreatedAtUtc = DateTimeOffset.UtcNow.AddMinutes(-5),
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.SaveAsync(expected);
        var actual = await repository.GetAsync();

        Assert.NotNull(actual);
        Assert.Equal(expected.GreenhouseName, actual.GreenhouseName);
        Assert.Equal(expected.GreenhouseLocation, actual.GreenhouseLocation);
        Assert.Equal(expected.Description, actual.Description);
    }

    private static string CreateTempFilePath()
    {
        return Path.Combine(Path.GetTempPath(), "greenhouse-tests", Guid.NewGuid().ToString("N"), "main-config.json");
    }
}
