using System.Net.Http;

namespace Amazon.S3;

public sealed class DeleteObjectRequest : S3Request
{
    public DeleteObjectRequest(string host, string bucketName, string key, string? versionId = null)
        : base(HttpMethod.Delete, host, bucketName, key, versionId: versionId)
    {
        ArgumentNullException.ThrowIfNull(key);

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }
}

// TODO
// x-amz-bypass-governance-retention
// x-amz-expected-bucket-owner
// x-amz-mfa
// x-amz-request-payer