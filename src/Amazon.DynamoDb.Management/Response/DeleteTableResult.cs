#nullable disable

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class DeleteTableResult
{
    public TableDescription TableDescription { get; init; }
}