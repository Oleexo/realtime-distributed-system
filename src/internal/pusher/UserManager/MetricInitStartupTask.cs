using Oleexo.RealtimeDistributedSystem.Common.Metrics;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

public class MetricInitStartupTask : IStartupTask {
    private readonly IMetrics _metrics;

    public MetricInitStartupTask(IMetrics metrics) {
        _metrics = metrics;
    }

    public string Name => "Metric init";

    public Task RunAsync(CancellationToken cancellationToken = default) {
        _metrics.CreateGauge(MetricConstants.ConnectedUsers, "Number of users connected currently.");
        return Task.CompletedTask;
    }
}

public static class MetricConstants {
    public const string ConnectedUsers = "users_connected";
}
