using System.Net.Http;

namespace Amazon.S3
{
    public sealed class CopyObjectRequest : S3Request
    {
        public CopyObjectRequest(string host, S3ObjectLocation source, S3ObjectLocation target)
            : base(HttpMethod.Put, host, target.BucketName, target.Key)
        {
            Headers.Add("x-amz-copy-source", $"/{source.BucketName}/{source.Key}");

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }
}