using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3
{
    public class GetObjectRequest : S3Request
    {
        public GetObjectRequest(AwsRegion region, string bucketName, string key)
            : base(HttpMethod.Get, region, bucketName, key)
        {
            CompletionOption = HttpCompletionOption.ResponseHeadersRead;
        }

        public void SetRange(long? from, long? to)
        {
            Headers.Range = new RangeHeaderValue(from, to);
        }
    }
}