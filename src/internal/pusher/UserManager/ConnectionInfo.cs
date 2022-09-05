using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

internal class ConnectionInfo {
    public ConnectionInfo(string              userId,
                          string              deviceId,
                          ConnectionId        connectionId,
                          ChannelFilter       filter,
                          DateTimeOffset      connectedAt,
                          Func<Message, Task> handler) {
        Id          = connectionId;
        UserId      = userId;
        DeviceId    = deviceId;
        Filter      = filter;
        ConnectedAt = connectedAt;
        Handler     = handler;
    }

    public ConnectionId        Id          { get; }
    public string              UserId      { get; }
    public string              DeviceId    { get; }
    public ChannelFilter       Filter      { get; }
    public DateTimeOffset      ConnectedAt { get; }
    public Func<Message, Task> Handler     { get; }
}