using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.Dispatch;

public record DispatchLetterCommand :  ICommand {
    public Letter Letter { get; init; } = Letter.Empty;
}