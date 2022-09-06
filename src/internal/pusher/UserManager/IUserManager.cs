using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

public interface IUserManager {
    Task<ConnectionId?> ConnectAsync(string              userId,
                                     string              deviceId,
                                     ChannelFilter       filter,
                                     Func<Message, Task> messageHandler,
                                     Func<Event, Task>   eventHandler);

    Task DisconnectAsync(ConnectionId connectionId);
    Task RefreshAllAsync();
    Task DispatchAsync(Letter   wrapper);
    void SetQueueInfo(QueueInfo queueInfo, string serverName);
    bool IsReady();
    Task DisconnectAllAsync(CancellationToken cancellationToken = default);
}
