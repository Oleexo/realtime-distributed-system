using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks;

internal sealed class StartupTaskManager : IStartupTaskStatus {
    private readonly ILogger<StartupTaskManager> _logger;

    public StartupTaskManager(ILogger<StartupTaskManager> logger) {
        _logger = logger;
    }

    public bool IsFinished { get; private set; }

    public async Task RunAsync(IServiceProvider  services,
                               CancellationToken cancellationToken = default) {
        using var scope = services.CreateScope();
        var       tasks = scope.ServiceProvider.GetServices<IStartupTask>();

        foreach (var task in tasks) {
            var sw = Stopwatch.StartNew();
            try {
                await task.RunAsync(cancellationToken);
            }
            catch (Exception e) {
                sw.Stop();
                _logger.LogError(e, "The task {TaskName} failed in {ElapsedMilliseconds}ms", task.Name, sw.ElapsedMilliseconds);
                throw;
            }

            _logger.LogInformation("The task {TaskName} was done in {ElapsedMilliseconds}ms", task.Name, sw.ElapsedMilliseconds);
        }

        IsFinished = true;
    }
}