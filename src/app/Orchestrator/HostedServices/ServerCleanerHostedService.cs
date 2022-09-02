﻿using MediatR;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.CleanDeadServer;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HostedServices; 

public sealed class ServerCleanerHostedService : IHostedService, IDisposable {
    private readonly IMediator                           _mediator;
    private readonly ILogger<ServerCleanerHostedService> _logger;
    private          Timer?                              _timer;

    public ServerCleanerHostedService(IMediator mediator, ILogger<ServerCleanerHostedService> logger) {
        _mediator    = mediator;
        _logger = logger;
    }

    private void CleanDeadServers(object? state) {
        var result = _mediator.Send(new CleanDeadServerCommand()).Result;
        if (result.IsFaulted) {
            _logger.LogWarning("Clean dead server failed");
        }
    }
    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Start clean dead server hosted service");
        _timer = new Timer(CleanDeadServers, null, TimeSpan.Zero,
                           TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken  cancellationToken) {
        _logger.LogInformation("Stop clean dead server hosted service");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() {
        _timer?.Dispose();
    }
}