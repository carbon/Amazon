using System;
using System.Net.Http;

namespace Amazon.S3
{
    public sealed class DeleteObjectRequest : S3Request
    {
        public DeleteObjectRequest(string host, string bucketName, string key, string version = null)
            : base(HttpMethod.Delete, host, bucketName, key, version)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
        }
    }
}