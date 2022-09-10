using System.Text.Json.Serialization;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;

public record EditMessageResponse {
    [JsonPropertyName("message_id")]
    public long MessageId { get; init; }
}