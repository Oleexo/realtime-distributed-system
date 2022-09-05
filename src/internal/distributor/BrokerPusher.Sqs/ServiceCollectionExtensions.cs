using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.Sqs;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSqsBrokerPusher(this IServiceCollection services,
                                                      IConfiguration          configuration) {
        services.Configure<SqsOptions>(configuration.GetSection("Aws"));
        services.AddSingleton<IBrokerPusher, SqsBrokerPusher>();

        return services;
    }
}
