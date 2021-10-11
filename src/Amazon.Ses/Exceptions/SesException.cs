using System.Net;

using Amazon.Exceptions;

namespace Amazon.Ses;

public sealed class SesException : AwsException
{
    private readonly SesError _error;

    public SesException(SesError error, HttpStatusCode statusCode)
        : base(error.Message, statusCode)
    {
        _error = error;
    }

    public string Type => _error.Type;

    public string Code => _error.Code;

    public bool IsTransient => HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable || Code is "Throttling";
}