using Greenhouse.Core.Configuration;

namespace Greenhouse.Core.Setup.Abstractions;

public interface IMainConfigRepository
{
    Task<MainConfig?> GetAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(MainConfig config, CancellationToken cancellationToken = default);
}
