using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Options;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

internal class DynamoDbClient : IDynamoDbContext {
    public DynamoDbClient(IOptions<DynamoDbOptions> options) {
        var credentials = new BasicAWSCredentials("Dummy", "Dummy");
        Instance = new AmazonDynamoDBClient(credentials, new AmazonDynamoDBConfig {
            ServiceURL = options.Value.Region
        });
    }

    public AmazonDynamoDBClient Instance { get; }
}
