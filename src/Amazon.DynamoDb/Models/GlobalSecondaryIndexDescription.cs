namespace Amazon.DynamoDb.Models
{
    public sealed class GlobalSecondaryIndexDescription : IndexDescription
    {
        public bool Backfilling { get; set; }

        public string? IndexArn { get; set; }

        public string? IndexStatus { get; set; }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
    }
}