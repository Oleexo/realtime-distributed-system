using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOrchestratorApi(this IServiceCollection services,
                                                        IConfiguration          configuration) {
        var url = configuration["Orchestrator:Url"];
        if (string.IsNullOrEmpty(url)) {
            throw new InvalidOperationException("Missing orchestrator url");
        }
        services.AddRefitClient<IOrchestratorApi>()
                .ConfigureHttpClient(o => o.BaseAddress = new Uri(url));
        return services;
    }
}
