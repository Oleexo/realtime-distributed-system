using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUserManager(this IServiceCollection services) {
        return services.AddSingleton<IUserManager, UserManager>()
                       .AddScoped<IStartupTask, MetricInitStartupTask>();
    }
}
