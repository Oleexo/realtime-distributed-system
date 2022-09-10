using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.DeleteMessage;

public record DeleteMessageCommand : ICommand<long> {
    public long                        MessageId  { get; init; }
    public string                      ChannelId  { get; init; } = string.Empty;
    public string                      Tag        { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();
}