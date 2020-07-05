using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class GlobalSecondaryIndex : LocalSecondaryIndex
    {
        public GlobalSecondaryIndex(string indexName, KeySchemaElement[] keySchema, Projection projection)
            : base(indexName, keySchema, projection)
        {
            
        }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }

        public new JsonObject ToJson()
        {
            JsonObject json = base.ToJson();

            if (ProvisionedThroughput != null) 
                json.Add("ProvisionedThroughput", ProvisionedThroughput.ToJson());

            return json;
        }
    }
}
