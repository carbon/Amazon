using System;
using System.Net;

namespace Amazon.S3
{
    public class S3Exception : Exception
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
                ErrorCode = s3Exception.ErrorCode;
                RequestId = s3Exception.RequestId;
                ResponseText = s3Exception.ResponseText;
            }
        }

        public S3Exception(S3Error error, HttpStatusCode httpStatusCode, string responseText)
            : this(error.Message, httpStatusCode)
        {
            ErrorCode = error.Code;
            RequestId = error.RequestId;
            ResponseText = responseText;
        }

        /// <summary>
        /// Gets status code returned by the service if available. If status
        /// code is set to -1, it means that status code was unavailable at the
        /// time exception was thrown
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        public string ErrorCode { get; }

        public string RequestId { get; }

        public string ResponseText { get; }

        public bool IsTransient => HttpStatusCode == HttpStatusCode.InternalServerError;
    }
}
