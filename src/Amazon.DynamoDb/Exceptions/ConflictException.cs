using System.Net;

namespace Amazon.DynamoDb;

public sealed class ConflictException(string message, HttpStatusCode statusCode) 
    : DynamoDbException(message, type: "ConditionalCheckFailedException", statusCode: statusCode)
{
}
