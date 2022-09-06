using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddDynamoDbPersistence(this IServiceCollection services,
                                                            IConfiguration          configuration) {
        return services.Configure<DynamoDbOptions>(configuration.GetSection("Aws"))
                       .AddSingleton<IDynamoDbContext, DynamoDbClient>()
                       .AddScoped<IStartupTask, RealtimeTableTask>()
                       .AddScoped<IStartupTask, MessageTableTask>();
    }
}
