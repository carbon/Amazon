using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class DeleteGlobalSecondaryIndexAction
    {
        public DeleteGlobalSecondaryIndexAction(string indexName)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
        }

        public string IndexName { get; }

        public JsonObject ToJson()
        {
            var json = new JsonObject
            {
                { "IndexName", IndexName },
            };

            return json;
        }
    }
}
