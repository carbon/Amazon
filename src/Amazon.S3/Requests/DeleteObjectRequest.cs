using System.Net.Http;

namespace Amazon.S3
{
    public class DeleteObjectRequest : S3Request
    {
        public DeleteObjectRequest(string bucketName, string key, string version = null)
            : base(HttpMethod.Delete, bucketName, key, version)
        { }
    }
}