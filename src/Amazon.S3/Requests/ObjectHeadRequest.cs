using System.Net.Http;

namespace Amazon.S3
{
    public class ObjectHeadRequest : S3Request
    {
        public ObjectHeadRequest(AwsRegion region, string bucketName, string key)
            : base(HttpMethod.Head, region, bucketName, key)
        {
        }
    }
}