using Amazon.S3;

namespace Wasabi;

public sealed class MoveObjectRequest : S3Request
{
    private static readonly HttpMethod s_MOVE = new ("MOVE");

    public MoveObjectRequest(string host, S3ObjectLocation source, string destinationKey)
        : base(s_MOVE, host, source.BucketName, source.Key)
    {
        Headers.TryAddWithoutValidation("Destination", destinationKey);
    }
}