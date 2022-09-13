using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks;

internal sealed class StartupHealthCheck : IHealthCheck {
    private readonly IStartupTaskStatus _startupTaskStatus;

    public StartupHealthCheck(IStartupTaskStatus startupTaskStatus) {
        _startupTaskStatus = startupTaskStatus;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                    CancellationToken  cancellationToken = default) {
        return Task.FromResult(_startupTaskStatus.IsFinished
                                   ? HealthCheckResult.Healthy()
                                   : HealthCheckResult.Unhealthy());
    }
}
