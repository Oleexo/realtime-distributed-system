using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

internal class ConnectionInfo
{
    public ConnectionInfo(string userId, string deviceId, ChannelFilter filter)
    {
        Id = ConnectionId.New(); 
        UserId = userId;
        DeviceId = deviceId;
        Filter = filter;
    }

    public ConnectionId Id { get; }
    public string UserId { get; }
    public string DeviceId { get; }
    public ChannelFilter Filter { get; }
}

public record ServiceOptions
{
    public string Name { get; init; } = string.Empty;
    public string OrchestratorUrl { get; init; } = string.Empty;
}

internal class PusherServerHostedService : IHostedService, IDisposable
{
    private readonly ConcurrentDictionary<ConnectionId, ConnectionInfo> _connections;

    public PusherServerHostedService(IOptions<ServiceOptions> options)
    {
        _connections = new ConcurrentDictionary<ConnectionId, ConnectionInfo>();
        _configuration = options.Value;
    }

    private Timer? _timer;
    private readonly ServiceOptions _configuration;

    public Task<ConnectionId> Connect(string userId, string deviceId, ChannelFilter filter)
    {
        var info = new ConnectionInfo(userId, deviceId, filter);
        if (!_connections.TryAdd(info.Id, info))
        {
                        
        }
        return Task.FromResult(info.Id);
    }

    public Task Disconnect(ConnectionId connectionId)
    {
        if (_connections.TryGetValue(connectionId, out var info))
        {
            _connections.TryRemove(connectionId, out var _);
        }
        return Task.CompletedTask;
    }

    private void RefreshConnectedUsers(object? state)
    {
        
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // todo register server to orchestrator
        
        _timer = new Timer(RefreshConnectedUsers, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}