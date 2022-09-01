using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;

public record RegisterPusherCommand : ICommand<RegisterPusherResult>
{
    public string Name { get; init; }
}

public record RegisterPusherResult
{
    public RegisterPusherResult(QueueType queueType,
                                string    queueName) {
        QueueType = queueType;
        QueueName = queueName;
    }

    public QueueType QueueType { get; }
    public string    QueueName { get; }
} 