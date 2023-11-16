namespace Amazon.DynamoDb.Models;

public sealed class GlobalSecondaryIndexDescription : IndexDescription
{
    public bool Backfilling { get; init; }

    public string? IndexArn { get; init; }

    public string? IndexStatus { get; init; }

    public ProvisionedThroughput? ProvisionedThroughput { get; init; }
}