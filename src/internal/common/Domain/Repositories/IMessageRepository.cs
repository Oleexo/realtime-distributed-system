using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;

public interface IMessageRepository {
    Task CreateAsync(Message           message,
                     CancellationToken cancellationToken = default);

    Task<Message?> GetByIdAsync(string            channelId,
                                long              messageId,
                                CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Message>> GetAllAsync(string            channelId,
                                                   GetAllOptions     options,
                                                   CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Message>> GetAllAsync(string            channelId,
                                                   CancellationToken cancellationToken = default);
}

public record GetAllOptions {
    public int  Limit       { get; init; } = 100;
    public long Offset      { get; init; } = 0;
    public bool ScanForward { get; init; } = false;
    public bool EventStyle  { get; init; } = true;
}
