using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Store.Commands.EditMessage;

public record EditMessageCommand : ICommand<long> {
    public string                      Content    { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Recipients { get; init; } = new List<string>();
    public string                      Tag        { get; init; } = string.Empty;
    public string                      ChannelId  { get; init; } = string.Empty;
    public long                        MessageId  { get; init; }
}