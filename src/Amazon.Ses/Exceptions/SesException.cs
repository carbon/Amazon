using System.Net;

using Amazon.Exceptions;

namespace Amazon.Ses;

public sealed class SesException(string message, HttpStatusCode statusCode)
    : AwsException(message, statusCode)
{
    public bool IsTransient => HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable; // || Code is "Throttling";
}

// CODES -- 

// AccessDeniedException
// IncompleteSignature
// InternalFailure
// InvalidAction
// InvalidClientTokenId
// InvalidParameterCombination
// InvalidParameterValue
// InvalidQueryParameter
// MalformedQueryString
// MissingAction
// MissingAuthenticationToken
// MissingParameter
// NotAuthorized
// OptInRequired
// RequestExpired
// ServiceUnavailable
// ThrottlingException
// ValidationError