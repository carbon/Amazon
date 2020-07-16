using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ReplicaGlobalSecondaryIndexDescription
    {
        public string? IndexName { get; set; }
        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
    }
}
