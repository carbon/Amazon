using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class GlobalSecondaryIndexDescription : IndexDescription
    {
        public bool Backfilling { get; set; }
        public string? IndexArn { get; set; }
        public string? IndexStatus { get; set; }
        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
    }
}
