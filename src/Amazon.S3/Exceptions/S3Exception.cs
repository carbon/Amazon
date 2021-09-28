using System;
using System.Net;

using Amazon.Scheduling;

namespace Amazon.S3
{
    // TODO: Inhert from AwsException

    public sealed class S3Exception : Exception, IException
    {
        public S3Exception(string message, HttpStatusCode statusCode)
            : base(message)
        {
            HttpStatusCode = statusCode;
        }

        public S3Exception(string message, Exception innerException)
            : base(message, innerException)
        {
            if (innerException is S3Exception s3Exception)
            {
                HttpStatusCode = s3Exception.HttpStatusCode;
                ErrorCode      = s3Exception.ErrorCode;
                RequestId      = s3Exception.RequestId;
            }
        }

        public S3Exception(S3Error error, HttpStatusCode statusCode)
            : this(error.Message, statusCode)
        {
            ErrorCode      = error.Code;
            HttpStatusCode = statusCode;
            RequestId      = error.RequestId;
        }

        public HttpStatusCode HttpStatusCode { get; }

        public string? ErrorCode { get; }

        public string? RequestId { get; }

        public bool IsTransient => HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable; // 500 || 503
    }
}
