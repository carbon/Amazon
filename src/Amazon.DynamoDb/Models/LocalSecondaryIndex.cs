using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class LocalSecondaryIndex
    {
        public LocalSecondaryIndex(string indexName, KeySchemaElement[] keySchema, Projection projection)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
            KeySchema = keySchema ?? throw new ArgumentNullException(nameof(keySchema));
            Projection = projection ?? throw new ArgumentNullException(nameof(projection));
        }

        public string IndexName { get; }
        public KeySchemaElement[] KeySchema { get; }
        public Projection Projection { get; }

        public JsonObject ToJson()
        {
            return new JsonObject
            {
                { "IndexName", IndexName },
                { "KeySchema", KeySchema.ToJson() },
                { "Projection", Projection.ToJson() }
            };
        }
    }
}
