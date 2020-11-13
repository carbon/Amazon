using System.Net;

using Amazon.Exceptions;

namespace Amazon.Ses
{
    public sealed class SesException : AwsException
    {
        private readonly SesError error;

        public SesException(SesError error, HttpStatusCode statusCode)
            : base(error.Message, statusCode)
        {
            this.error = error;
        }

        public string Type => error.Type;

        public string Code => error.Code;

        public bool IsTransient => HttpStatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable || Code is "Throttling";
    }
}