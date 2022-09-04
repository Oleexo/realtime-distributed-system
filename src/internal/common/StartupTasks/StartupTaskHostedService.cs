using Microsoft.Extensions.Hosting;

namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks;

internal sealed class StartupTaskHostedService : IHostedService {
    private readonly IServiceProvider   _services;
    private readonly StartupTaskManager _startupTaskManager;

    public StartupTaskHostedService(IServiceProvider   services,
                                    StartupTaskManager startupTaskManager) {
        _services           = services;
        _startupTaskManager = startupTaskManager;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        return _startupTaskManager.RunAsync(_services, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
