using System.Net;

using Amazon.Exceptions;
using Amazon.Scheduling;

namespace Amazon.Sns;

public sealed class SnsException(string message, HttpStatusCode httpStatusCode, string? code = null, string? requestId = null)
    : AwsException(message, httpStatusCode), IException
{
    // e.g. NotFound, AuthorizationError, InvalidParameter
    public string? Code { get; } = code;

    public string? RequestId { get; } = requestId;

    public bool IsTransient { get; } = (int)httpStatusCode is 429 or >= 500 || code is "Throttled" or "Throttling" or "ThrottlingException" or "ThrottledException";
}

// https://docs.aws.amazon.com/sns/latest/api/CommonErrors.html
