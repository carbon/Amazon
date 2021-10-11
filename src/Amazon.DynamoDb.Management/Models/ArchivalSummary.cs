namespace Amazon.DynamoDb.Models;

public sealed class ArchivalSummary
{
    public string? ArchivalBackupArn { get; init; }

    public string? ArchivalDateTime { get; init; }

    public string? ArchivalReason { get; init; }
}