using System;
using System.Net.Http;

namespace Amazon.S3
{
    public sealed class DeleteObjectRequest : S3Request
    {
        public DeleteObjectRequest(string host, string bucketName, string key, string? versionId = null)
            : base(HttpMethod.Delete, host, bucketName, key, versionId: versionId)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }
}