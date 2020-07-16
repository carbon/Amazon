using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class GlobalSecondaryIndexDescription : IndexDescription
    {
        public bool Backfilling { get; set; }
        public string? IndexArn { get; set; }
        public string? IndexStatus { get; set; }
        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
    }
}
