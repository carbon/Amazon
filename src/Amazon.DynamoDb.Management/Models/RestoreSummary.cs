namespace Amazon.DynamoDb.Models;

public sealed class RestoreSummary
{
    public Timestamp RestoreDateTime { get; init; }

    public bool RestoreInProgress { get; init; }

    public string? SourceBackupArn { get; init; }
}
