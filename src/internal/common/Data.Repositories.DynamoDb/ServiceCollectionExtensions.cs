using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddCommonRepositories(this IServiceCollection services) {
        return services.AddScoped<IUserConnectionRepository, UserConnectionRepository>()
                       .AddScoped<IMessageRepository, MessageRepository>();
    }
}
