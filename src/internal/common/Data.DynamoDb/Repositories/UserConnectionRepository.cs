using System.Globalization;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb.Repositories;

internal abstract class BaseRepository<T> : DynamoDbStorage {
    protected BaseRepository(string  tableName,
                             ILogger logger)
        : base(tableName, logger) {
    }

    protected abstract Dictionary<string, AttributeValue> ToFields(T entity);
    protected abstract string GetHashKey(string                      id);

    protected string Serialize(object obj) {
        return JsonSerializer.Serialize(obj);
    }
}

internal class UserConnectionRepository : BaseRepository<UserConnection>, IUserConnectionRepository {
    private readonly DynamoDbClient _dynamoDbClient;

    public UserConnectionRepository(DynamoDbClient                    dynamoDbClient,
                                    ILogger<UserConnectionRepository> logger)
        : base("realtime", logger) {
        _dynamoDbClient = dynamoDbClient;
    }

    public async Task<bool> CreateAsync(UserConnection    userConnection,
                                        CancellationToken cancellationToken = default) {
        await PutEntryAsync(ToFields(userConnection), cancellationToken: cancellationToken);
        return true;
    }

    public async Task DeleteAsync(string            id,
                                  CancellationToken cancellationToken = default) {
        var key = GetHashKey(id);
        await DeleteEntryAsync(new Dictionary<string, AttributeValue> {
            { "PK", new AttributeValue { S = key } },
            { "SK", new AttributeValue { S = key } }
        }, cancellationToken: cancellationToken);
    }

    public Task UpdateLastSeenAsync(string            id,
                                    DateTime          lastSeenValue,
                                    CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }
    
    protected override Dictionary<string, AttributeValue> ToFields(UserConnection entity) {
        var key = GetHashKey(entity.Id);
        return new Dictionary<string, AttributeValue> {
            { "PK", new AttributeValue { S           = key } },
            { "SK", new AttributeValue { S           = key } },
            { "user_id", new AttributeValue { S      = entity.UserId } },
            { "device_id", new AttributeValue { S    = entity.DeviceId } },
            { "filter", new AttributeValue { S       = Serialize(entity.Filter) } },
            { "connected_at", new AttributeValue { S = entity.ConnectedAt.ToString("O", CultureInfo.InvariantCulture) } },
            { "last_seen", new AttributeValue { S    = entity.LastSeen.ToString("O", CultureInfo.InvariantCulture) } },
            { "queue", new AttributeValue { S        = Serialize(entity.Queue)} }
        };
    }

    protected override string GetHashKey(string id) {
        return $"UserConnection#{id}";
    }

    protected override AmazonDynamoDBClient DbClient => _dynamoDbClient.Instance;
}
