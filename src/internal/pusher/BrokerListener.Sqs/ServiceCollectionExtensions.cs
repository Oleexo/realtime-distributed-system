using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddBrokerListener(this IServiceCollection services,
                                                       IConfiguration          configuration) {
        services.Configure<SqsOptions>(configuration.GetSection("Broker"));
        services.AddSingleton<IBrokerListener, SqsBrokerListener>();

        return services;
    }
}
