using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

internal class UserManager : IUserManager {
    private readonly IServiceProvider                                   _services;
    private readonly ConcurrentDictionary<ConnectionId, ConnectionInfo> _connections;
    private          QueueInfo?                                         _queueInfo;

    public UserManager(IServiceProvider services) {
        _services = services;
        _connections   = new ConcurrentDictionary<ConnectionId, ConnectionInfo>();
    }

    public async Task<ConnectionId?> ConnectAsync(string              userId,
                                                  string              deviceId,
                                                  ChannelFilter       filter,
                                                  Func<Message, Task> messageHandler) {
        if (_queueInfo is null) {
            throw new InvalidOperationException("Queue info not set");
        }
        using var scope                    = _services.CreateScope();
        var       userConnectionRepository = GetUserConnectionRepository(scope);

        var current = new UserConnection {
            DeviceId    = deviceId,
            UserId      = userId,
            Filter      = filter,
            ConnectedAt = DateTime.UtcNow,
            LastSeen    = DateTime.UtcNow,
            Queue       = _queueInfo
        };
        var info = new ConnectionInfo(userId, deviceId, new ConnectionId(current.Id), filter, current.ConnectedAt, messageHandler);
        if (!_connections.TryAdd(info.Id, info)) {
            return null;
        }

        await userConnectionRepository.CreateAsync(current);
        return info.Id;
    }
    
    public async Task RefreshAllAsync() {
        if (_queueInfo is null) {
            throw new InvalidOperationException("Queue info not set");
        }
        using var scope                    = _services.CreateScope();
        var       userConnectionRepository = GetUserConnectionRepository(scope);

        foreach (var info in _connections.Values) {
            var current = new UserConnection {
                DeviceId    = info.DeviceId,
                UserId      = info.UserId,
                Queue       = _queueInfo,
                Filter      = info.Filter,
                ConnectedAt = info.ConnectedAt,
                LastSeen    = DateTimeOffset.UtcNow
            };
            await userConnectionRepository.UpdateAsync(current);
        }
    }
    
    private static IUserConnectionRepository GetUserConnectionRepository(IServiceScope serviceScope) {
        return serviceScope.ServiceProvider.GetRequiredService<IUserConnectionRepository>();
    }
    public async Task DisconnectAsync(ConnectionId connectionId) {
        using var scope                    = _services.CreateScope();
        var       userConnectionRepository = GetUserConnectionRepository(scope);
        _connections.TryRemove(connectionId, out _);
        await userConnectionRepository.DeleteAsync(connectionId.Value);
    }



    public async Task DispatchAsync(MessageWrapper wrapper) {
        foreach (var recipient in wrapper.Recipients.Distinct()) {
            var info = _connections.Values.FirstOrDefault(p => p.UserId == recipient);
            // todo better search with dictionary
            if (info is not null) {
                if (info.Filter.Tags.Any(tag => wrapper.Tags.Contains(tag))) {
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

    public async Task DisconnectAllAsync(CancellationToken cancellationToken = default) {
        using var scope                    = _services.CreateScope();
        var       userConnectionRepository = GetUserConnectionRepository(scope);

        foreach (var info in _connections.Values) {
            await userConnectionRepository.DeleteAsync(info.Id.Value, cancellationToken);
        }
    }
}
