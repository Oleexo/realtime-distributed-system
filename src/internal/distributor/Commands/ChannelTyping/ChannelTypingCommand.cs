using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Commands.ChannelTyping;

public record ChannelTypingCommand : ICommand {
    public string                      UserId     { get; init; } = string.Empty;
    public string                      ChannelId  { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = Array.Empty<string>();
    public string                      Tag        { get; init; } = string.Empty;
    public bool                        IsTyping   { get; init; } 
}