using System.Net.Http;

namespace Amazon.S3
{
    public sealed class ListVersionsRequest : S3Request
    {
        public ListVersionsRequest(string host, string bucketName, ListVersionsOptions options)
            : base(HttpMethod.Get, host, bucketName, "versions", options.Items)
        {
            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }
}