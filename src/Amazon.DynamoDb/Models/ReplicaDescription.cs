using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class ReplicaDescription : IConvertibleFromJson
    {
        public ReplicaGlobalSecondaryIndexDescription[]? GlobalSecondaryIndexes { get; set; }
        public string? KMSMasterKeyId { get; set; }
        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
        public string? RegionName { get; set; }
        public ReplicaStatus ReplicaStatus { get; set; }
        public string? ReplicaStatusDescription { get; set; }
        public string? ReplicaStatusPercentProgress { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("GlobalSecondaryIndexes")) GlobalSecondaryIndexes = property.Value.GetObjectArray<ReplicaGlobalSecondaryIndexDescription>();
            else if (property.NameEquals("KMSMasterKeyId")) KMSMasterKeyId = property.Value.GetString();
            else if (property.NameEquals("ProvisionedThroughputOverride")) ProvisionedThroughputOverride = property.Value.GetObject<ProvisionedThroughputOverride>();
            else if (property.NameEquals("RegionName")) RegionName = property.Value.GetString();
            else if (property.NameEquals("ReplicaStatus")) ReplicaStatus = property.Value.GetEnum<ReplicaStatus>();
            else if (property.NameEquals("ReplicaStatusDescription")) ReplicaStatusDescription = property.Value.GetString();
            else if (property.NameEquals("ReplicaStatusPercentProgress")) ReplicaStatusPercentProgress = property.Value.GetString();
        }
    }
}
