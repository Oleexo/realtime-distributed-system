using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;

internal sealed class MessageRepository : BaseRepository<Message>, IMessageRepository {
    public MessageRepository(IDynamoDbContext           dynamoDbContext,
                             ILogger<MessageRepository> logger)
        : base("messages", dynamoDbContext, logger) {
    }

    public Task CreateAsync(Message           message,
                            CancellationToken cancellationToken = default) {
        return PutEntryAsync(ToFields(message), cancellationToken: cancellationToken);
    }

    public Task<Message?> GetByIdAsync(string            channelId,
                                       long              messageId,
                                       CancellationToken cancellationToken = default) {
        return ReadSingleEntryAsync($"Channel#{channelId}", $"Message#{messageId}", FromFields, cancellationToken);
    }

    public Task<IReadOnlyCollection<Message>> GetAllAsync(string            channelId,
                                                          GetAllOptions     options,
                                                          CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Message>> GetAllAsync(string            channelId,
                                                          CancellationToken cancellationToken = default) {
        return GetAllAsync(channelId, new GetAllOptions(), cancellationToken);
    }

    private static Message FromFields(Dictionary<string, AttributeValue> fields) {
        return new Message {
            Content = fields["content"]
               .S,
            Id = long.Parse(fields["message_id"]
                               .N),
            ChannelId = fields["channel_id"]
               .S,
            ParentId = fields.ContainsKey("parent_id")
                           ? long.Parse(fields["parent_id"]
                                           .N)
                           : null,
            IsDeletion = fields.ContainsKey("is_deletion")
                             ? fields["is_deletion"]
                                .BOOL
                             : null
        };
    }

    private static Dictionary<string, AttributeValue> ToFields(Message message) {
        var fields = new Dictionary<string, AttributeValue> {
            { "PK", new AttributeValue { S         = $"Channel#{message.ChannelId}" } },
            { "SK", new AttributeValue { S         = $"Message#{message.Id}" } },
            { "message_id", new AttributeValue { N = message.Id.ToString() } },
            { "channel_id", new AttributeValue { S = message.ChannelId } },
            { "content", new AttributeValue { S    = message.Content } }
        };
        if (message.ParentId.HasValue) {
            fields.Add("parent_id", new AttributeValue { N = message.ParentId.Value.ToString() });
        }

        if (message.IsDeletion.HasValue) {
            fields.Add("is_deletion", new AttributeValue { BOOL = message.IsDeletion.Value });
        }

        return fields;
    }
}
