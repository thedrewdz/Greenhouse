using Greenhouse.Core.Setup;
using Greenhouse.Core.Setup.Abstractions;

namespace Greenhouse.UI.Infrastructure;

public sealed class DevelopmentNetworkService : INetworkService
{
    private bool isConnected;

    public Task<bool> IsConnectedAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(isConnected);
    }

    public Task<NetworkConnectionResult> ConnectAsync(
        NetworkCredentials credentials,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(credentials.NetworkName))
        {
            return Task.FromResult(NetworkConnectionResult.Failure("Network name is required."));
        }

        isConnected = true;
        return Task.FromResult(NetworkConnectionResult.Success());
    }
}
