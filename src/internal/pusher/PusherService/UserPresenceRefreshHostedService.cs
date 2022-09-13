using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

internal class UserPresenceRefreshHostedService : IHostedService, IDisposable {
    private readonly ILogger<UserPresenceRefreshHostedService> _logger;
    private readonly IUserManager                              _userManager;

    private Timer? _timer;

    public UserPresenceRefreshHostedService(IUserManager                              userManager,
                                            ILogger<UserPresenceRefreshHostedService> logger) {
        _userManager = userManager;
        _logger      = logger;
    }

    public void Dispose() {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _timer = new Timer(RefreshConnectedUsers, null, TimeSpan.FromSeconds(5),
                           TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _timer?.Change(Timeout.Infinite, 0);
        return _userManager.DisconnectAllAsync(cancellationToken);
    }

    private void RefreshConnectedUsers(object? state) {
        try {
            _userManager.RefreshAllAsync()
                        .GetAwaiter()
                        .GetResult();
            _logger.LogDebug("User presence refresh done");
        }
        catch (Exception e) {
            _logger.LogError(e, "User presence loop failed to refresh");
        }
    }
}
