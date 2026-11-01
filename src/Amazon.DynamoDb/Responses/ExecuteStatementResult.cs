namespace Amazon.DynamoDb;

public sealed class ExecuteStatementResult : IConsumedResources
{
    /// <summary>
    /// Empty for statements that do not return items, such as an INSERT
    /// </summary>
    public AttributeCollection[] Items { get; init; } = [];

    /// <summary>
    /// When present, there may be more results. Send it in a new request with the same statement and parameters.
    /// </summary>
    public string? NextToken { get; init; }

    public Dictionary<string, DbValue>? LastEvaluatedKey { get; init; }

    public ConsumedCapacity? ConsumedCapacity { get; init; }
}
