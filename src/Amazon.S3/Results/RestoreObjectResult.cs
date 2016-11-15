using System.Net;
using System.Net.Http;

namespace Amazon.S3
{
    public class RestoreObjectResult
    {
        private readonly HttpResponseMessage response;

        public RestoreObjectResult(HttpResponseMessage response)
        {
            this.response = response;
        }

        public HttpStatusCode StatusCode => response.StatusCode;
    }
}

/*
HTTP/1.1 202 Accepted
x-amz-id-2: GFihv3y6+kE7KG11GEkQhU7/2/cHR3Yb2fCb2S04nxI423Dqwg2XiQ0B/UZlzYQvPiBlZNRcovw=
x-amz-request-id: 9F341CD3C4BA79E0
Date: Sat, 20 Oct 2012 23:54:05 GMT
Content-Length: 0
Server: AmazonS3
*/
