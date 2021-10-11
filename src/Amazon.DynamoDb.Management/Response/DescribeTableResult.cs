#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class DescribeTableResult
{
    public TableDescription Table { get; init; }
}