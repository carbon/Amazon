using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class GlobalSecondaryIndex : LocalSecondaryIndex
    {
        public GlobalSecondaryIndex(string indexName, KeySchemaElement[] keySchema, Projection projection)
            : base(indexName, keySchema, projection)
        {
            
        }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
    }
}
