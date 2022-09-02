using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Data.InMemory;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddPersistence(this IServiceCollection services) {
        return services.AddSingleton<IPusherServerRepository, PusherServerRepository>();
    }
}
