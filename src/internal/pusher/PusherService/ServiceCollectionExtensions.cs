using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUserPresenceSystem(this IServiceCollection services,
                                                           IConfiguration          configuration) {
        return services.AddHostedService<MessageListenerHostedService>()
                       .AddHostedService<UserPresenceRefreshHostedService>()
                       .AddBrokerListener(configuration);
    }

    public static IServiceCollection AddUserPresenceHealthCheck(this IServiceCollection services) {
        services.AddHealthChecks()
                       .AddCheck<UserManagerHealthCheck>("UserManager",
                                                         failureStatus: HealthStatus.Unhealthy,
                                                         new[] { "ready" });
        return services;
    }
}
