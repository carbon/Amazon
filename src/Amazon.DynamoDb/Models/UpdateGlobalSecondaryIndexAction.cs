using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class UpdateGlobalSecondaryIndexAction
    {
        public UpdateGlobalSecondaryIndexAction(string indexName, ProvisionedThroughput provisionedThroughput)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
            ProvisionedThroughput = provisionedThroughput ?? throw new ArgumentNullException(nameof(provisionedThroughput));
        }

        public string IndexName { get; }
        public ProvisionedThroughput ProvisionedThroughput { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject
            {
                { "IndexName", IndexName },
                { "ProvisionedThroughput", ProvisionedThroughput.ToJson() },
            };

            return json;
        }
    }
}
