using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class CreateGlobalSecondaryIndexAction
    {
        public CreateGlobalSecondaryIndexAction(string indexName, KeySchemaElement[] keySchema, Projection projection)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
            KeySchema = keySchema ?? throw new ArgumentNullException(nameof(keySchema));
            Projection = projection ?? throw new ArgumentNullException(nameof(projection));
        }

        public string IndexName { get; }
        public KeySchemaElement[] KeySchema { get; }
        public Projection Projection { get; }
        public ProvisionedThroughput? ProvisionedThroughput { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject
            {
                { "IndexName", IndexName },
                { "KeySchema", KeySchema.ToJson() },
                { "Projection", Projection.ToJson() }
            };

            if (ProvisionedThroughput != null) json.Add("ProvisionedThroughput", ProvisionedThroughput.ToJson());

            return json;
        }
    }
}
