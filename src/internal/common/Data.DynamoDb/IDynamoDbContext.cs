using Amazon.DynamoDBv2;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

public interface IDynamoDbContext {
    AmazonDynamoDBClient Instance { get; }
}
