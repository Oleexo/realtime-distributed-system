using System.Globalization;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Data.Repositories.DynamoDb;

public class PusherServerRepository : BaseRepository<PusherServer>, IPusherServerRepository {
    private const string PrimaryKey = "Orchestrator#PusherServer";

    public PusherServerRepository(IDynamoDbContext                dynamoDbContext,
                                  ILogger<PusherServerRepository> logger)
        : base("realtime", dynamoDbContext, logger) {
    }

    public async Task<bool> CreateAsync(PusherServer      pusherServer,
                                        CancellationToken cancellationToken = default) {
        await PutEntryAsync(ToFields(pusherServer), cancellationToken: cancellationToken);
        return true;
    }

    public async Task<PusherServer?> GetByIdAsync(string            name,
                                                  CancellationToken cancellationToken = default) {
        var key = GetHashKey(name);
        return await ReadSingleEntryAsync(PrimaryKey, key, ToEntity, cancellationToken);
    }

    public async Task<IReadOnlyCollection<PusherServer>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await ReadEntriesAsync("#PK = :PK",
                                      new Dictionary<string, AttributeValue> {
                                          { "PK", new AttributeValue { S = PrimaryKey } }
                                      }, ToEntity, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(PusherServer      current,
                                        CancellationToken cancellationToken = default) {
        await PutEntryAsync(ToFields(current), cancellationToken: cancellationToken);
        return true;
    }

    public Task DeleteAsync(string            id,
                            CancellationToken cancellationToken = default) {
        var key = GetHashKey(id);
        return DeleteEntryAsync(new Dictionary<string, AttributeValue> {
            { "PK", new AttributeValue { S = PrimaryKey } },
            { "SK", new AttributeValue { S = key } }
        }, cancellationToken: cancellationToken);
    }

    private static PusherServer ToEntity(Dictionary<string, AttributeValue> fields) {
        var queue = new QueueInfo(Enum.Parse<QueueType>(fields["queue_type"]
                                                           .S), fields["queue_name"]
                                     .S);
        var data = new Data {
            Id = fields["server_name"]
               .S,
            Queue = queue,
            CreatedAt = DateTimeOffset.Parse(fields["created_at"]
                                                .S),
            LastSeen = DateTimeOffset.Parse(fields["last_seen"]
                                               .S)
        };
        return new PusherServer(data);
    }

    private Dictionary<string, AttributeValue> ToFields(PusherServer entity) {
        var key = GetHashKey(entity.Id);
        return new Dictionary<string, AttributeValue> {
            { "PK", new AttributeValue { S          = PrimaryKey } },
            { "SK", new AttributeValue { S          = key } },
            { "server_name", new AttributeValue { S = entity.Id } },
            { "queue_type", new AttributeValue { S  = entity.Queue.Type.ToString() } },
            { "queue_name", new AttributeValue { S  = entity.Queue.Name } },
            { "created_at", new AttributeValue { S  = entity.CreatedAt.ToString("O", CultureInfo.InvariantCulture) } },
            { "last_seen", new AttributeValue { S   = entity.LastSeen.ToString("O", CultureInfo.InvariantCulture) } }
        };
    }

    private string GetHashKey(string id) {
        return $"PusherServer#{id}";
    }

    private record Data : PusherServer.IData {
        public string         Id        { get; init; } = string.Empty;
        public QueueInfo      Queue     { get; init; } = QueueInfo.Empty;
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset LastSeen  { get; init; }
    }
}
