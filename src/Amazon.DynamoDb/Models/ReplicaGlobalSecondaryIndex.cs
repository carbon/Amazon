using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class ReplicaGlobalSecondaryIndex
    {
        public ReplicaGlobalSecondaryIndex(string indexName)
        {
            IndexName = indexName;
        }

        public string IndexName;
        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject
            {
                { "IndexName", IndexName }
            };

            if (ProvisionedThroughputOverride != null)
                json.Add("ProvisionedThroughputOverride", ProvisionedThroughputOverride.ToJson());

            return json;
        }
    }
}
