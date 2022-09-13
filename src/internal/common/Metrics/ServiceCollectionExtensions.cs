using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Common.Metrics;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddMetricService(this IServiceCollection services) {
        return services.AddScoped<IMetrics, PrometheusMetrics>();
    }
}