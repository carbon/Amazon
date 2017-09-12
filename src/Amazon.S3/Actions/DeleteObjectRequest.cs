using System.Net.Http;

namespace Amazon.S3
{
    public class DeleteObjectRequest : S3Request
    {
        public DeleteObjectRequest(string host, string bucketName, string key, string version = null)
            : base(HttpMethod.Delete, host, bucketName, key, version)
        { }
    }
}