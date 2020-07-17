namespace Amazon.DynamoDb
{
    public abstract class ReplicationGroupMemberAction
    {
        public ReplicationGroupMemberAction(string regionName)
        {
            RegionName = regionName;
        }

        public string RegionName { get; }

        public ReplicaGlobalSecondaryIndex[]? GlobalSecondaryIndexes { get; set; }

        public string? KMSMasterKeyId { get; set; }

        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
    }
}