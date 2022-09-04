using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

internal class DatabaseCreatorTask : DynamoDbStorage, IStartupTask {
    private readonly IDynamoDbContext _dynamoDbContext;

    public DatabaseCreatorTask(IDynamoDbContext             dynamoDbContext,
                               ILogger<DatabaseCreatorTask> logger)
        : base("realtime", logger) {
        _dynamoDbContext = dynamoDbContext;
    }

    public string Name => "DynamoDB Init tables";

    public async Task RunAsync(CancellationToken cancellationToken = default) {
        await InitializeTable(new List<KeySchemaElement> {
            new KeySchemaElement("PK", KeyType.HASH),
            new KeySchemaElement("SK", KeyType.RANGE)
        }, new List<AttributeDefinition> {
            new AttributeDefinition("PK", ScalarAttributeType.S),
            new AttributeDefinition("SK", ScalarAttributeType.S),
        });
    }

    protected override AmazonDynamoDBClient DbClient => _dynamoDbContext.Instance;
}
