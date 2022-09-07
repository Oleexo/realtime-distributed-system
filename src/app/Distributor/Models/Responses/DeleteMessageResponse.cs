using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Responses;

public record DeleteMessageResponse {
    [JsonPropertyName("message_id")]
    public long MessageId { get; init; }
}
