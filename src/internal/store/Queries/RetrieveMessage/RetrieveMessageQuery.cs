using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMessage;

public record RetrieveMessageQuery : IQuery<Message> {
    public long   MessageId { get; init; }
    public string ChannelId { get; init; } = string.Empty;
}