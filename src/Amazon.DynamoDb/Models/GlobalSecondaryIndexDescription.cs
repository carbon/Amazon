using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public sealed class GlobalSecondaryIndexDescription : IndexDescription, IConvertibleFromJson
    {
        public bool Backfilling { get; set; }
        public string? IndexArn { get; set; }
        public string? IndexStatus { get; set; }
        public ProvisionedThroughput? ProvisionedThroughput { get; set; }

        public new void FillField(JsonProperty prop)
        {
            base.FillField(prop);
            if (prop.NameEquals("Backfilling")) Backfilling = prop.Value.GetBoolean();
            else if (prop.NameEquals("IndexArn")) IndexArn = prop.Value.GetString();
            else if (prop.NameEquals("IndexStatus")) IndexStatus = prop.Value.GetString();
            else if (prop.NameEquals("ProvisionedThroughput")) ProvisionedThroughput = prop.Value.GetObject<ProvisionedThroughput>();
        }
    }
}
