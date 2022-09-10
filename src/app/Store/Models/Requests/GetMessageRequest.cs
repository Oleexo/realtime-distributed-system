using System.Text.Json.Serialization;
using Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;

public record GetMessageRequest : IHttpRequestModel {
    [JsonPropertyName("message_id")]
    public long   MessageId { get; private set; }
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; private set; } = string.Empty;

    public void PopulateFromContext(HttpRequest httpRequest) {
        if (httpRequest.Query.TryGetValue("message_id", out var messageIdStr)) {
           MessageId = long.Parse(messageIdStr);
        }

        if (httpRequest.Query.TryGetValue("channel_id", out var channelId)) {
            ChannelId = channelId;
        }
    }
}