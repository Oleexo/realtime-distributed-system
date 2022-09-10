using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Responses;

public record DispatchMessageResponse {
    [JsonPropertyName("message_id")]
    public long MessageId { get; init; }
}
