using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Store.Queries.GetMessage;

public record RetrieveMessageQuery : IQuery<Message> {
    public long   MessageId { get; init; }
    public string ChannelId { get; init; } = string.Empty;
}