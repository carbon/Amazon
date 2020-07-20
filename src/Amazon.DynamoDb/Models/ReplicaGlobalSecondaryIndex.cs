namespace Amazon.DynamoDb
{
    public sealed class ReplicaGlobalSecondaryIndex
    {
        public ReplicaGlobalSecondaryIndex(string indexName)
        {
            IndexName = indexName;
        }

        public string IndexName;

        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
    }
}