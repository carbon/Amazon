using System.Net;

using Amazon.Exceptions;

namespace Amazon.Ses;

public sealed class SesException(SesError error, HttpStatusCode statusCode)
    : AwsException(error.Message, statusCode)
{
    private readonly SesError _error = error;

    public string Type => _error.Type;

    public string Code => _error.Code;

    public bool IsTransient => HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable || Code is "Throttling";
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