#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class CreateTableResult
{
    public TableDescription TableDescription { get; init; }
}