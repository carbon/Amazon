using System.Net.Http;

namespace Amazon.S3
{
    public class DeleteObjectRequest : S3Request
    {
        public DeleteObjectRequest(AwsRegion region, string bucketName, string key, string version = null)
            : base(HttpMethod.Delete, region, bucketName, key, version)
        { }
    }
}