using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUserManager(this IServiceCollection services) {
        return services.AddSingleton<IUserManager, UserManager>();
    }
}
