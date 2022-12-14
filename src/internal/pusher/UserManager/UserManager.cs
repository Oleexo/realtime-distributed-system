using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Repositories;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.Common.Metrics;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

internal class UserManager : IUserManager {
    private readonly ConcurrentDictionary<ConnectionId, ConnectionInfo> _connections;
    private readonly IServiceProvider                                   _services;
    private readonly IMetrics                                           _metrics;
    private          QueueInfo?                                         _queueInfo;
    private          string?                                            _serverName;

    public UserManager(IServiceProvider services,
                       IMetrics metrics) {
        _services     = services;
        _metrics = metrics;
        _connections  = new ConcurrentDictionary<ConnectionId, ConnectionInfo>();
    }

    public async Task<ConnectionId?> ConnectAsync(string              userId,
                                                  string              deviceId,
                                                  ChannelFilter       filter,
                                                  Func<Message, Task> messageHandler,
                                                  Func<Event, Task>   eventHandler) {
        if (_queueInfo is null ||
            string.IsNullOrEmpty(_serverName)) {
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
            Queue       = _queueInfo,
            ServerName  = _serverName
        };
        var info = new ConnectionInfo(userId,
                                      deviceId,
                                      new ConnectionId(current.Id),
                                      filter,
                                      current.ConnectedAt,
                                      messageHandler,
                                      eventHandler);
        if (!_connections.TryAdd(info.Id, info)) {
            return null;
        }

        await userConnectionRepository.CreateAsync(current);
        _metrics.IncrGauge(MetricConstants.ConnectedUsers);
        return info.Id;
    }

    public async Task RefreshAllAsync() {
        if (_queueInfo is null ||
            string.IsNullOrEmpty(_serverName)) {
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
                LastSeen    = DateTimeOffset.UtcNow,
                ServerName  = _serverName
            };
            await userConnectionRepository.UpdateAsync(current);
        }
    }

    public async Task DisconnectAsync(ConnectionId connectionId) {
        try {
            using var scope                    = _services.CreateScope();
            var       userConnectionRepository = GetUserConnectionRepository(scope);
            _connections.TryRemove(connectionId, out _);
            await userConnectionRepository.DeleteAsync(connectionId.Value);
        }
        finally {
            _metrics.DecrGauge(MetricConstants.ConnectedUsers);
        }
    }

    public async Task DispatchAsync(Letter wrapper) {
        foreach (var recipient in wrapper.Recipients.Distinct()) {
            var info = _connections.Values.FirstOrDefault(p => p.UserId == recipient);
            // todo better search with dictionary
            if (info is not null) {
                if (info.Filter.Tags.Any(tag => wrapper.Tag.Contains(tag))) {
                    switch (wrapper.Type) {
                        case LetterType.Message when wrapper.Message is not null:
                            await info.MessageHandler(wrapper.Message);
                            break;
                        case LetterType.Event when wrapper.Event is not null:
                            await info.EventHandler(wrapper.Event);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }

    public void SetQueueInfo(QueueInfo queueInfo,
                             string    serverName) {
        _queueInfo  = queueInfo;
        _serverName = serverName;
    }

    public bool IsReady() {
        return _queueInfo is not null;
    }

    public async Task DisconnectAllAsync(CancellationToken cancellationToken = default) {
        using var scope                    = _services.CreateScope();
        var       userConnectionRepository = GetUserConnectionRepository(scope);

        foreach (var connectionId in new List<ConnectionId>(_connections.Keys)) {
            if (_connections.TryRemove(connectionId, out var info)) {
                await userConnectionRepository.DeleteAsync(info.Id.Value);
                _metrics.DecrGauge(MetricConstants.ConnectedUsers);
            }
        }
    }

    private static IUserConnectionRepository GetUserConnectionRepository(IServiceScope serviceScope) {
        return serviceScope.ServiceProvider.GetRequiredService<IUserConnectionRepository>();
    }
}
