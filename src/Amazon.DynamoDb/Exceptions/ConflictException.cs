using System.Net;

namespace Amazon.DynamoDb;

public sealed class ConflictException : DynamoDbException
{
    public ConflictException(string message, HttpStatusCode statusCode)
        : base(message, type: "ConditionalCheckFailedException", statusCode: statusCode) { }
}
