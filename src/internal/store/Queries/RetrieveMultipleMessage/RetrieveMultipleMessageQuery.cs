using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMultipleMessage;

public record RetrieveMultipleMessageQuery : IQuery<IReadOnlyCollection<Message>> {
    public string ChannelId   { get; init; } = string.Empty;
    public int    Limit       { get; init; }
    public long   Offset      { get; init; }
    public bool   ScanForward { get; init; }
    public bool   EventStyle  { get; init; }
}
