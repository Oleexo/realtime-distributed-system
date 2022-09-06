using System.Globalization;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;

internal class UserConnectionRepository : BaseRepository<UserConnection>, IUserConnectionRepository {
    private readonly IDynamoDbContext _dynamoDbContext;

    public UserConnectionRepository(IDynamoDbContext                  dynamoDbContext,
                                    ILogger<UserConnectionRepository> logger)
        : base("realtime", dynamoDbContext, logger) {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<bool> CreateAsync(UserConnection    userConnection,
                                        CancellationToken cancellationToken = default) {
        await CreateOrUpdateAsync(userConnection, cancellationToken);
        return true;
    }

    private async Task CreateOrUpdateAsync(UserConnection    userConnection,
                                           CancellationToken cancellationToken) {
        var key = GetHashKey(userConnection.Id);
        var ttl = userConnection.LastSeen.AddSeconds(60)
                                .ToUnixTimeSeconds();

        var principal = ToDocument(userConnection, key, ttl);
        var entries   = new List<Document> { principal };
        foreach (var tag in userConnection.Filter.Tags) {
            var document = new Document(principal.ToDictionary(p => p.Key, p => p.Value)) {
                ["SK"]      = $"Tag#{tag}",
                ["GSI_PK1"] = $"Tag#{tag}",
                ["GSI_SK1"] = key
            };
            entries.Add(document);
        }

        await PutEntriesAsync(entries, cancellationToken: cancellationToken);
    }

    public Task DeleteAsync(string            id,
                            CancellationToken cancellationToken = default) {
        var key = GetHashKey(id);
        return DeletePartitionAsync("PK", key, "SK", cancellationToken);
    }

    public Task UpdateAsync(UserConnection    userConnection,
                            CancellationToken cancellationToken = default) {
        return CreateOrUpdateAsync(userConnection, cancellationToken);
    }

    public Task<IReadOnlyCollection<UserConnection>> GetConnectedUsersWithTag(string            tag,
                                                                              CancellationToken cancellationToken = default) {
        return ReadEntriesAsync("#GSI_PK1 = :GSI_PK1", new Dictionary<string, AttributeValue> {
            { "GSI_PK1", new AttributeValue { S = $"Tag#{tag}" } }
        }, ToEntity, "gsi_1", cancellationToken: cancellationToken);
    }

    private static UserConnection ToEntity(Dictionary<string, AttributeValue> fields) {
        var queue = new QueueInfo(Enum.Parse<QueueType>(fields["queue_type"]
                                                           .S), fields["queue_name"]
                                     .S);
        return new UserConnection {
            UserId = fields["user_id"]
               .S,
            DeviceId = fields["device_id"]
               .S,
            ConnectedAt = DateTimeOffset.Parse(fields["connected_at"]
                                                  .S),
            LastSeen = DateTimeOffset.Parse(fields["last_seen"]
                                               .S),
            Queue = queue,
            Filter = new ChannelFilter {
                Tags = new[] {
                    fields["GSI_PK1"]
                       .S
                }
            }
        };
    }

    private static Document ToDocument(UserConnection entity,
                                       string         key,
                                       long           ttl) {
        return new Document {
            { "PK", key },
            { "SK", key },
            { "user_id", entity.UserId },
            { "device_id", entity.DeviceId },
            { "connected_at", entity.ConnectedAt.ToString("O", CultureInfo.InvariantCulture) },
            { "last_seen", entity.LastSeen.ToString("O", CultureInfo.InvariantCulture) },
            { "queue_type", entity.Queue.Type.ToString() },
            { "queue_name", entity.Queue.Name },
            { "server_name", entity.ServerName },
            { "ttl", ttl }
        };
    }

    private static string GetHashKey(string id) {
        return $"UserConnection#{id}";
    }

    protected override AmazonDynamoDBClient DbClient => _dynamoDbContext.Instance;
}
