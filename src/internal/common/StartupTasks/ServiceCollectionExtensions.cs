using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddStartupTasks(this IServiceCollection services) {
        services.AddHealthChecks()
                .AddCheck<StartupHealthCheck>("StartupTasks", HealthStatus.Unhealthy, new[] { "ready" });
        return services.AddHostedService<StartupTaskHostedService>()
                       .AddSingleton<StartupTaskManager>()
                       .AddScoped<IStartupTaskStatus>(s => s.GetRequiredService<StartupTaskManager>());
    }
}
