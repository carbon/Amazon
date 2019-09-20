using System;
using System.Net;

namespace Amazon.S3
{
    public sealed class S3Exception : Exception
    {
        public S3Exception(string message, HttpStatusCode statusCode)
            : base(message)
        {
            HttpStatusCode = statusCode;
        }

        public S3Exception(string message, Exception ex)
            : base(message, ex)
        {
            if (ex is S3Exception s3Exception)
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

        /// <summary>
        /// Gets status code returned by the service if available. If status
        /// code is set to -1, it means that status code was unavailable at the
        /// time exception was thrown
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        public string? ErrorCode { get; }

        public string? RequestId { get; }

        public bool IsTransient => HttpStatusCode == HttpStatusCode.InternalServerError || HttpStatusCode == HttpStatusCode.ServiceUnavailable; // 500 || 503
    }
}
