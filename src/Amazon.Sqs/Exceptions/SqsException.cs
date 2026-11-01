using System.Net;

using Amazon.Scheduling;

using Carbon.Messaging;

namespace Amazon.Sqs.Exceptions;

public sealed class SqsException(ErrorResult error, HttpStatusCode? statusCode = null) 
    : QueueException(error.Message), IException
{
    public ErrorResult Error { get; } = error;

    public string Type => Error.Type;

    public HttpStatusCode? StatusCode { get; } = statusCode;

    public bool IsTransient => (int?)StatusCode >= 500 || IsThrottlingError(Error.Type);

    // Throttling is reported with a 400 or 403
    private static bool IsThrottlingError(string? type)
    {
        return type is not null && (
            type.EndsWith("RequestThrottled", StringComparison.Ordinal) ||
            type.EndsWith("ThrottlingException", StringComparison.Ordinal) ||
            type.EndsWith("KmsThrottled", StringComparison.Ordinal));
    }
}