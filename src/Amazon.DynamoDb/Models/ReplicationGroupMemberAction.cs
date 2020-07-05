using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
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

        public JsonObject ToJson()
        {
            var json = new JsonObject()
            {
                { "RegionName", RegionName }
            };

            if (GlobalSecondaryIndexes != null)
                json.Add("GlobalSecondaryIndexes", GlobalSecondaryIndexes.ToJson());

            if (KMSMasterKeyId != null)
                json.Add("KMSMasterKeyId", KMSMasterKeyId);

            if (ProvisionedThroughputOverride != null)
                json.Add("ProvisionedThroughputOverride", ProvisionedThroughputOverride.ToJson());

            return json;
        }
    }
}
