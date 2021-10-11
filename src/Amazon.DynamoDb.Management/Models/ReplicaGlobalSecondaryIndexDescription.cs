namespace Amazon.DynamoDb.Models;

public sealed class ReplicaGlobalSecondaryIndexDescription
{
    public string? IndexName { get; set; }

    public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
}
