using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;

public record Channel {
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("user_infos")]
    public Dictionary<string, ChannelUserInfo> UserInfos { get; init; } = new();
}

public record ChannelUserInfo {
    [JsonPropertyName("last_message_read")]
    public long LastMessageRead { get; set; }
}
