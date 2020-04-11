using System.Net.Http;

namespace Amazon.S3
{
    public sealed class ListBucketRequest : S3Request
    {
        public ListBucketRequest(string host, string bucketName, ListBucketOptions options)
            : base(HttpMethod.Get, host, bucketName, options.Items)
        {
            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }
}

/*
GET ?prefix=photos/2006/&delimiter=/ HTTP/1.1
Host: quotes.s3.amazonaws.com
Date: Wed, 01 Mar  2009 12:00:00 GMT
Authorization: AWS 15B4D3461F177624206A:xQE0diMbLRepdf3YB+FIEXAMPLE=
*/