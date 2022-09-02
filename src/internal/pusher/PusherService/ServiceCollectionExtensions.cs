using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener.AmazonSqs;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUserPresenceSystem(this IServiceCollection services, IConfiguration configuration) {
        return services.AddHostedService<MessageListenerHostedService>()
                       .AddHostedService<UserPresenceRefreshHostedService>()
                       .AddBrokerListener(configuration);
    }
}
