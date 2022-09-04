namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks.Abstractions;

public interface IStartupTask {
    string Name { get; }
    Task RunAsync(CancellationToken cancellationToken = default);
}
