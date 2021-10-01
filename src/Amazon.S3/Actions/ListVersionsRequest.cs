using System.Net.Http;

namespace Amazon.S3;

public sealed class ListVersionsRequest : S3Request
{
    public ListVersionsRequest(string host, string bucketName, ListVersionsOptions options)
        : base(HttpMethod.Get, host, bucketName, options.Items, actionName: S3ActionName.Versions)
    {
        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }
}