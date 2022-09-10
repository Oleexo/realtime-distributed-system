using System.Text.Json.Serialization;
using Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;

public record GetAllMessageRequest : IHttpRequestModel {
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; private set; } = string.Empty;

    [JsonPropertyName("limit")]
    public int Limit { get; private set; }

    [JsonPropertyName("offset")]
    public long Offset { get; private set; }

    [JsonPropertyName("scan_forward")]
    public bool ScanForward { get; private set; }

    [JsonPropertyName("event_style")]
    public bool EventStyle { get; private set; }

    public void PopulateFromContext(HttpRequest httpRequest) {
        if (httpRequest.Query.TryGetValue("channel_id", out var channelId)) {
            ChannelId = channelId;
        }

        if (httpRequest.Query.TryGetValue("limit", out var limitStr)) {
            if (int.TryParse(limitStr, out var limit) &&
                limit <= 100) {
                Limit = limit;
            }
            else {
                Limit = 100;
            }
        }

        if (httpRequest.Query.TryGetValue("offset", out var offsetStr)) {
            if (int.TryParse(offsetStr, out var offset) &&
                offset >= 0) {
                Offset = offset;
            }
            else {
                Offset = 0;
            }
        }

        if (httpRequest.Query.TryGetValue("scan_forward", out var scanForwardStr)) {
            if (bool.TryParse(scanForwardStr, out var scanForward)) {
                ScanForward = scanForward;
            }
            else {
                ScanForward = false;
            }
        }

        if (httpRequest.Query.TryGetValue("event_style", out var eventStyleStr)) {
            if (bool.TryParse(eventStyleStr, out var eventStyle)) {
                EventStyle = eventStyle;
            }
            else {
                EventStyle = true;
            }
        }
    }
}
