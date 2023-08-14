using System.Net;

namespace Amazon.S3;

public sealed class RestoreObjectResult(HttpStatusCode statusCode)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}

/*
HTTP/1.1 202 Accepted
Date: Sat, 20 Oct 2012 23:54:05 GMT
Content-Length: 0
Server: AmazonS3
*/
