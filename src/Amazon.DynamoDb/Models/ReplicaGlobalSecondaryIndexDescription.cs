using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class ReplicaGlobalSecondaryIndexDescription
    {
        public string? IndexName { get; set; }
        public ProvisionedThroughputOverride? ProvisionedThroughputOverride { get; set; }
    }
}
