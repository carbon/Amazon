using System.Net;

namespace Amazon.S3
{
    public sealed class RestoreObjectResult
    {
        public RestoreObjectResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}

/*
HTTP/1.1 202 Accepted
Date: Sat, 20 Oct 2012 23:54:05 GMT
Content-Length: 0
Server: AmazonS3
*/
