namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;

public record MessageResponse {
    public long   MessageId  { get; init; }
    public string ChannelId  { get; init; } = string.Empty;
    public string Content    { get; init; } = string.Empty;
    public long?  ParentId   { get; init; }
    public bool?  IsDeletion { get; init; }
}
