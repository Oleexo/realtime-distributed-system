using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks;

internal sealed class StartupTaskManager : IStartupTaskStatus {
    public bool IsFinished { get; private set; }

    public async Task RunAsync(IServiceProvider  services,
                               CancellationToken cancellationToken = default) {
        var logger = services.GetRequiredService<ILogger<StartupTaskManager>>();
        logger.LogDebug("Start startup tasks");
        using var scope = services.CreateScope();
        var       tasks = scope.ServiceProvider.GetServices<IStartupTask>();

        foreach (var task in tasks) {
            var sw = Stopwatch.StartNew();
            try {
                await task.RunAsync(cancellationToken);
            }
            catch (Exception e) {
                sw.Stop();
                logger.LogError(e, "The task {TaskName} failed in {ElapsedMilliseconds}ms", task.Name, sw.ElapsedMilliseconds);
                throw;
            }

            logger.LogInformation("The task {TaskName} was done in {ElapsedMilliseconds}ms", task.Name, sw.ElapsedMilliseconds);
        }

        IsFinished = true;
        logger.LogDebug("End startup tasks");
    }
}
