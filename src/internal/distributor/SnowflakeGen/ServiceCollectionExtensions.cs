using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Distributor.SnowflakeGen;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSnowflakeGen(this IServiceCollection services) {
        return services.AddSingleton<ISnowflakeGen, SnowflakeGenerator>();
    }
}