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
    }
}
