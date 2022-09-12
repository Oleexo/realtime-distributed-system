using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.ReadMessage;

public record ReadMessageCommand : ICommand {
    public string                      ChannelId  { get; init; } = string.Empty;
    public string                      Tag        { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();
    public long                        MessageId  { get; init; }
    public string                      UserId     { get; init; } = string.Empty;
}
