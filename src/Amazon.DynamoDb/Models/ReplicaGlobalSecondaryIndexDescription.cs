namespace Amazon.DynamoDb
{
    public sealed class ReplicaGlobalSecondaryIndexDescription
    {
        public string? IndexName { get; set; }

        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
    }
}