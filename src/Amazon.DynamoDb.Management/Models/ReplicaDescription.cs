using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

public sealed class ReplicaDescription
{
    public ReplicaGlobalSecondaryIndexDescription[]? GlobalSecondaryIndexes { get; init; }

    [JsonPropertyName("KMSMasterKeyId")]
    public string? KmsMasterKeyId { get; init; }

    public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; init; }

    public string? RegionName { get; init; }

    public ReplicaStatus ReplicaStatus { get; init; }

    public string? ReplicaStatusDescription { get; init; }

    public string? ReplicaStatusPercentProgress { get; init; }
}
