using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddCommonDynamoDbPersistence(this IServiceCollection services,
                                                                  IConfiguration          configuration) {
        return services.Configure<DynamoDbOptions>(configuration.GetSection("Aws"))
                       .AddScoped<IUserConnectionRepository, UserConnectionRepository>();
    }
}

internal class DynamoDbClient {
    public DynamoDbClient(IOptions<DynamoDbOptions> options) {
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        Instance = new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig {
            ServiceURL = options.Value.Region
        });
    }

    public AmazonDynamoDBClient Instance { get; }
}
