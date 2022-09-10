using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.StoreMessage;

public record StoreMessageCommand : ICommand<long> {
    public string                      ChannelId  { get; init; } = string.Empty;
    public string                      Content    { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();
    public string                      Tag        { get; init; } = string.Empty;
}
