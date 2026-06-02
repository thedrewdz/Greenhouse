using Greenhouse.Core.Onboarding.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Greenhouse.Bluetooth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBluetoothDiscovery(this IServiceCollection services)
    {
        services.AddScoped<IEdgeUnitDiscoveryService, BlueZEdgeUnitDiscoveryService>();
        return services;
    }
}
