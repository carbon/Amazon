namespace Amazon.DynamoDb.Models;

public sealed class ReplicaGlobalSecondaryIndex
{
    public ReplicaGlobalSecondaryIndex(string indexName)
    {
        IndexName = indexName;
    }

    public string IndexName { get; init; }

    public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; init; }
}
