using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ReplicaDescription
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
