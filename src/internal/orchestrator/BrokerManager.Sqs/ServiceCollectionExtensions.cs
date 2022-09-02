using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager.AmazonSqs;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSqsBrokerService(this IServiceCollection services,
                                                      IConfiguration          configuration) {
        services.Configure<SqsOptions>(configuration.GetSection("Aws"));
        services.AddSingleton<IBrokerService, SqsBrokerService>();

        return services;
    }
}
