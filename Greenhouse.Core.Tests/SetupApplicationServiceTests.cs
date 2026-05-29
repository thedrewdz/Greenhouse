using Greenhouse.Core.Configuration;
using Greenhouse.Core.Setup;
using Greenhouse.Core.Setup.Abstractions;

namespace Greenhouse.Core.Tests;

public sealed class SetupApplicationServiceTests
{
    [Fact]
    public async Task GetStatusAsync_ReturnsNetworkConnection_WhenConfigIsMissingAndNetworkDisconnected()
    {
        var service = CreateService(config: null, isConnected: false);

        var status = await service.GetStatusAsync();

        Assert.False(status.IsSetupComplete);
        Assert.Equal(SetupStartStep.NetworkConnection, status.StartStep);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsGeneralInformation_WhenConfigIsMissingAndNetworkConnected()
    {
        var service = CreateService(config: null, isConnected: true);

        var status = await service.GetStatusAsync();

        Assert.False(status.IsSetupComplete);
        Assert.Equal(SetupStartStep.GeneralInformation, status.StartStep);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsComplete_WhenConfigExists()
    {
        var service = CreateService(
            new MainConfig
            {
                GreenhouseName = "Orchid Palace",
                GreenhouseLocation = "Back Garden",
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            },
            isConnected: false);

        var status = await service.GetStatusAsync();

        Assert.True(status.IsSetupComplete);
        Assert.Equal(SetupStartStep.Complete, status.StartStep);
    }

    [Fact]
    public void ValidateNetworkCredentials_AllowsBlankPassword()
    {
        var service = CreateService(config: null, isConnected: false);

        var result = service.ValidateNetworkCredentials(new NetworkCredentials("open-network", ""));

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task WriteGeneralConfigurationAsync_TrimsAndStoresValidConfiguration()
    {
        var repository = new InMemoryMainConfigRepository(null);
        var service = new SetupApplicationService(repository, new StubNetworkService(false));

        var result = await service.WriteGeneralConfigurationAsync(
            new GeneralConfigurationRequest(" Orchid Palace ", " Back Garden ", " Hydroponics "));

        Assert.True(result.IsValid);
        Assert.NotNull(repository.Config);
        Assert.Equal("Orchid Palace", repository.Config.GreenhouseName);
        Assert.Equal("Back Garden", repository.Config.GreenhouseLocation);
        Assert.Equal("Hydroponics", repository.Config.Description);
    }

    [Fact]
    public async Task WriteGeneralConfigurationAsync_RejectsMissingRequiredValues()
    {
        var repository = new InMemoryMainConfigRepository(null);
        var service = new SetupApplicationService(repository, new StubNetworkService(false));

        var result = await service.WriteGeneralConfigurationAsync(new GeneralConfigurationRequest("", "", null));

        Assert.False(result.IsValid);
        Assert.Contains("Greenhouse name is required.", result.Errors);
        Assert.Contains("Greenhouse location is required.", result.Errors);
        Assert.Null(repository.Config);
    }

    private static SetupApplicationService CreateService(MainConfig? config, bool isConnected)
    {
        return new SetupApplicationService(
            new InMemoryMainConfigRepository(config),
            new StubNetworkService(isConnected));
    }

    private sealed class InMemoryMainConfigRepository(MainConfig? config) : IMainConfigRepository
    {
        public MainConfig? Config { get; private set; } = config;

        public Task<MainConfig?> GetAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Config);
        }

        public Task SaveAsync(MainConfig config, CancellationToken cancellationToken = default)
        {
            Config = config;
            return Task.CompletedTask;
        }
    }

    private sealed class StubNetworkService(bool isConnected) : INetworkService
    {
        public Task<bool> IsConnectedAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(isConnected);
        }

        public Task<NetworkConnectionResult> ConnectAsync(
            NetworkCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(NetworkConnectionResult.Success());
        }
    }
}
