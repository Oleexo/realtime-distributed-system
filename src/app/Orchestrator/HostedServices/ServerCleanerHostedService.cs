using MediatR;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.CleanDeadServer;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HostedServices;

public sealed class ServerCleanerHostedService : IHostedService, IDisposable {
    private readonly ILogger<ServerCleanerHostedService> _logger;
    private readonly IMediator                           _mediator;
    private          Timer?                              _timer;
    private readonly ServiceOptions                      _configuration;

    public ServerCleanerHostedService(IMediator                           mediator,
                                      IOptions<ServiceOptions>            options,
                                      ILogger<ServerCleanerHostedService> logger) {
        _mediator      = mediator;
        _configuration = options.Value;
        _logger        = logger;
    }

    public void Dispose() {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Start clean dead server hosted service");
        _timer = new Timer(CleanDeadServers, null, TimeSpan.FromSeconds(_configuration.StartInterval),
                           TimeSpan.FromSeconds(_configuration.Interval));
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
                                  .GetAwaiter()
                                  .GetResult();
            if (result.IsFaulted) {
                _logger.LogWarning("Clean dead server failed");
            }
        }
        catch (Exception e) {
            _logger.LogWarning(e, "Clean dead server failed");
        }
    }
}
