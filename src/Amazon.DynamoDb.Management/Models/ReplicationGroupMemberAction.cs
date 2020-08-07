using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models
{
    public abstract class ReplicationGroupMemberAction
    {
        public ReplicationGroupMemberAction(string regionName)
        {
            RegionName = regionName;
        }

        public string RegionName { get; }

        public ReplicaGlobalSecondaryIndex[]? GlobalSecondaryIndexes { get; set; }

        [JsonPropertyName("KMSMasterKeyId")]
        public string? KmsMasterKeyId { get; set; }

        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
    }
}