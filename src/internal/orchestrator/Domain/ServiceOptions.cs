namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Domain;

public record ServiceOptions {
    /// <summary>
    ///     Start interval in seconds
    /// </summary>
    public int StartInterval { get; init; } = 60;

    /// <summary>
    ///     Interval between clean in seconds
    /// </summary>
    public int Interval { get; init; } = 60;

    /// <summary>
    ///     Pusher are declared dead after interval in seconds
    /// </summary>
    public int PusherDeadAfterInterval { get; init; } = 60 * 60 * 24;
}
