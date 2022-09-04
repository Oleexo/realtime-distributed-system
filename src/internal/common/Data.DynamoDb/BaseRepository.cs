using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

public abstract class BaseRepository<T> : DynamoDbStorage {
    private readonly IDynamoDbContext _dynamoDbContext;

    protected BaseRepository(string           tableName,
                             IDynamoDbContext dynamoDbContext,
                             ILogger          logger)
        : base(tableName, logger) {
        _dynamoDbContext = dynamoDbContext;
    }

    protected override AmazonDynamoDBClient DbClient => _dynamoDbContext.Instance;
    protected abstract Dictionary<string, AttributeValue> ToFields(T entity);
    protected abstract string GetHashKey(string                      id);

    protected static string Serialize(object obj) {
        return JsonSerializer.Serialize(obj);
    }
}