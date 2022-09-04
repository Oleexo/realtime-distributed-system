using System.Collections.Concurrent;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

internal class UserManager : IUserManager {
    private readonly ConcurrentDictionary<ConnectionId, ConnectionInfo> _connections;
    private readonly IUserConnectionRepository                          _userConnectionRepository;
    private          QueueInfo?                                         _queueInfo;

    public UserManager(IUserConnectionRepository userConnectionRepository) {
        _userConnectionRepository = userConnectionRepository;
        _connections              = new ConcurrentDictionary<ConnectionId, ConnectionInfo>();
    }

    public async Task<ConnectionId?> ConnectAsync(string              userId,
                                                  string              deviceId,
                                                  ChannelFilter       filter,
                                                  Func<Message, Task> messageHandler) {
        if (_queueInfo is null) {
            throw new InvalidOperationException("Queue info not set");
        }

        var current = new UserConnection {
            DeviceId    = deviceId,
            UserId      = userId,
            Filter      = filter,
            ConnectedAt = DateTime.UtcNow,
            LastSeen    = DateTime.UtcNow,
            Queue       = _queueInfo
        };
        var info = new ConnectionInfo(userId, deviceId, new ConnectionId(current.Id), filter, messageHandler);
        if (!_connections.TryAdd(info.Id, info)) {
            return null;
        }

        await _userConnectionRepository.CreateAsync(current);
        return info.Id;
    }

    public async Task DisconnectAsync(ConnectionId connectionId) {
        _connections.TryRemove(connectionId, out _);
        await _userConnectionRepository.DeleteAsync(connectionId.Value);
    }

    public async Task RefreshAllAsync() {
        foreach (var info in _connections.Values) {
            await _userConnectionRepository.UpdateLastSeenAsync(info.Id.Value, DateTime.UtcNow);
        }
    }

    public async Task DispatchAsync(MessageWrapper wrapper) {
        foreach (var recipient in wrapper.Recipients.Distinct()) {
            var info = _connections.Values.FirstOrDefault(p => p.UserId == recipient);
            // todo better search with dictionary
            if (info is not null) {
                if (info.Filter.Tags.All(tag => wrapper.Tags.Contains(tag))) {
                    await info.Handler(wrapper.Message);
                }
            }
        }
    }

    public void SetQueueInfo(QueueInfo queueInfo) {
        _queueInfo = queueInfo;
    }

    public bool IsReady() {
        return _queueInfo is not null;
    }
}
