namespace Oleexo.RealtimeDistributedSystem.Common.StartupTasks;

internal interface IStartupTaskStatus {
    bool IsFinished { get; }
}