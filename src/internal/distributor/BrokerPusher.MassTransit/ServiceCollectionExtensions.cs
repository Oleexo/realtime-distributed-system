using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.MassTransit;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSqsBrokerPusher(this IServiceCollection services) {
        services.AddSingleton<IBrokerPusher, MassTransitBrokerPush>();

        return services;
    }
}
