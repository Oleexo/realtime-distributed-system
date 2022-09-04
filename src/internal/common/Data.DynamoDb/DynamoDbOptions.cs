namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

internal record DynamoDbOptions {
    public string Region { get; init; } = string.Empty;
}