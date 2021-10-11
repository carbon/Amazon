namespace Amazon.DynamoDb.Models;

public abstract class IndexDescription
{
    public string? IndexName { get; init; }

    public long IndexSizeBytes { get; init; }

    public long ItemCount { get; init; }

    public KeySchemaElement[]? KeySchema { get; init; }

    public Projection? Projection { get; init; }
}
