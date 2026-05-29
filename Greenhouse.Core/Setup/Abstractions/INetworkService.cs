namespace Greenhouse.Core.Setup.Abstractions;

public interface INetworkService
{
    Task<bool> IsConnectedAsync(CancellationToken cancellationToken = default);

    Task<NetworkConnectionResult> ConnectAsync(
        NetworkCredentials credentials,
        CancellationToken cancellationToken = default);
}
