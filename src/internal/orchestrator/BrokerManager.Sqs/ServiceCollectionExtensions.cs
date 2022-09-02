using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager.AmazonSqs;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddBrokerService(this IServiceCollection services,
                                                      IConfiguration          configuration) {
        services.Configure<SqsOptions>(configuration.GetSection("Broker"));
        services.AddSingleton<IBrokerService, SqsBrokerService>();

        return services;
    }
}
