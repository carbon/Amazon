using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class ReplicationGroupMemberAction
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
