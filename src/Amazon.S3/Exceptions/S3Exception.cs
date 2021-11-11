using System.Net;

using Amazon.Exceptions;
using Amazon.Scheduling;

namespace Amazon.S3;

public sealed class S3Exception : AwsException, IException
{
    private readonly S3Error? _error;

    public S3Exception(string message, HttpStatusCode statusCode)
        : base(message, statusCode) { }

    public S3Exception(string message, Exception innerException, HttpStatusCode statusCode)
        : base(message, innerException, statusCode)
    {
        if (innerException is S3Exception s3Exception)
        {
            _error = s3Exception.Error;
        }
    }

    public S3Exception(S3Error error, HttpStatusCode statusCode)
        : this(error.Message, statusCode)
    {
        _error = error;
    }

    public S3Error? Error => _error;

    public bool IsTransient => HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable; // 500 || 503
}