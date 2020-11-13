using System.Net;

using Amazon.Exceptions;

namespace Amazon.Route53.Exceptions
{
    public sealed class Route53Exception : AwsException
    {
        public Route53Exception(string message, HttpStatusCode statusCode)
            : base(message, statusCode) { }
    }
}
