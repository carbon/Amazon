namespace Amazon.DynamoDb.Models
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