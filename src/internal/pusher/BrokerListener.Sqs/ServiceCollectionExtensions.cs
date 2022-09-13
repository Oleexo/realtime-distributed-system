using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSqsBrokerListener(this IServiceCollection services,
                                                          IConfiguration          configuration) {
        services.Configure<SqsOptions>(configuration.GetSection("Aws"));
        services.AddSingleton<IBrokerListener, SqsBrokerListener>();

        return services;
    }
}
