using MediatR;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.CleanDeadServer;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HostedServices;

public sealed class ServerCleanerHostedService : IHostedService, IDisposable {
    private readonly ILogger<ServerCleanerHostedService> _logger;
    private readonly IMediator                           _mediator;
    private          Timer?                              _timer;

    public ServerCleanerHostedService(IMediator                           mediator,
                                      ILogger<ServerCleanerHostedService> logger) {
        _mediator = mediator;
        _logger   = logger;
    }

    public void Dispose() {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Start clean dead server hosted service");
        _timer = new Timer(CleanDeadServers, null, TimeSpan.FromSeconds(5),
                           TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Stop clean dead server hosted service");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void CleanDeadServers(object? state) {
        try {
            var result = _mediator.Send(new CleanDeadServerCommand())
                                  .Result;
            if (result.IsFaulted) {
                _logger.LogWarning("Clean dead server failed");
            }
        }
        catch (Exception e) {
            _logger.LogWarning(e,"Clean dead server failed");
        }
    }
}
