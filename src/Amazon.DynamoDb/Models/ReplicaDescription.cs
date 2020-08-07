namespace Amazon.DynamoDb.Models
{
    public sealed class ReplicaDescription
    {
        public ReplicaGlobalSecondaryIndexDescription[]? GlobalSecondaryIndexes { get; set; }

        public string? KMSMasterKeyId { get; set; }

        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }

        public string? RegionName { get; set; }

        public ReplicaStatus ReplicaStatus { get; set; }

        public string? ReplicaStatusDescription { get; set; }

        public string? ReplicaStatusPercentProgress { get; set; }
    }
}
