using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

internal sealed class MessageTableTask : DynamoDbStorage, IStartupTask {
    private readonly IDynamoDbContext _dynamoDbContext;

    public MessageTableTask(IDynamoDbContext          dynamoDbContext,
                            ILogger<MessageTableTask> logger)
        : base("messages", logger) {
        _dynamoDbContext = dynamoDbContext;
    }

    protected override AmazonDynamoDBClient DbClient => _dynamoDbContext.Instance;
    public             string               Name     => "DynamoDB Init messages table";

    public Task RunAsync(CancellationToken cancellationToken = default) {
        return InitializeTable(new List<KeySchemaElement> {
            new("PK", KeyType.HASH),
            new("SK", KeyType.RANGE)
        }, new List<AttributeDefinition> {
            new("PK", ScalarAttributeType.S),
            new("SK", ScalarAttributeType.S)
        });
    }
}

internal sealed class RealtimeTableTask : DynamoDbStorage, IStartupTask {
    private readonly IDynamoDbContext _dynamoDbContext;

    public RealtimeTableTask(IDynamoDbContext           dynamoDbContext,
                             ILogger<RealtimeTableTask> logger)
        : base("realtime", logger) {
        _dynamoDbContext = dynamoDbContext;
    }

    protected override AmazonDynamoDBClient DbClient => _dynamoDbContext.Instance;

    public string Name => "DynamoDB Init realtime table";

    public Task RunAsync(CancellationToken cancellationToken = default) {
        return InitializeTable(new List<KeySchemaElement> {
                                   new("PK", KeyType.HASH),
                                   new("SK", KeyType.RANGE)
                               }, new List<AttributeDefinition> {
                                   new("PK", ScalarAttributeType.S),
                                   new("SK", ScalarAttributeType.S)
                               },
                               new List<GlobalSecondaryIndex> {
                                   new() {
                                       Projection = null,
                                       IndexName  = "gsi_1",
                                       KeySchema = new List<KeySchemaElement> {
                                           new("GSI_PK1", KeyType.HASH),
                                           new("GSI_SK1", KeyType.RANGE)
                                       },
                                       ProvisionedThroughput = null
                                   }
                               },
                               "ttl");
    }
}
