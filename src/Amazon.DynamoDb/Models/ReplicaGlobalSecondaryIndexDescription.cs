using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ReplicaGlobalSecondaryIndexDescription : IConvertibleFromJson
    {
        public string? IndexName { get; set; }
        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("IndexName")) IndexName = property.Value.GetString();
            else if (property.NameEquals("ProvisionedThroughputOverride")) ProvisionedThroughputOverride = property.Value.GetObject<ProvisionedThroughputOverride>();
        }
    }
}
