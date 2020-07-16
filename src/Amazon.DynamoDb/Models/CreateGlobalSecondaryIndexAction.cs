using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
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
    }
}
