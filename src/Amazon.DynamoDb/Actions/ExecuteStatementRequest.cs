namespace Amazon.DynamoDb;

public sealed class ExecuteStatementRequest
{
    public ExecuteStatementRequest(string statement, params DbValue[]? parameters)
    {
        ArgumentException.ThrowIfNullOrEmpty(statement);

        Statement = statement;

        // DynamoDB requires at least one parameter when they are present
        Parameters = parameters is { Length: > 0 } ? parameters : null;
    }

    /// <summary>
    /// The PartiQL statement. Pass values as parameters (?) rather than formatting them into the text.
    /// </summary>
    public string Statement { get; }

    public DbValue[]? Parameters { get; }

    public bool? ConsistentRead { get; set; }

    public int? Limit { get; set; }

    public string? NextToken { get; set; }

    public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

    public ReturnValuesOnConditionCheckFailure? ReturnValuesOnConditionCheckFailure { get; set; }
}

// https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_ExecuteStatement.html
