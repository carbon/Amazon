namespace Amazon;

internal static class XAmzContentSha256Values
{
    public const string UnsignedPayload               = "UNSIGNED-PAYLOAD";
    public const string StreamingAwsHmacSha256Payload = "STREAMING-AWS4-HMAC-SHA256-PAYLOAD";
}

// https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-streaming-trailers.html