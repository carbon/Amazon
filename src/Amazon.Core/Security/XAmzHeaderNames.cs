namespace Amazon.Security;

internal static class XAmzHeaderNames
{
    public const string ContentSHA256        = "x-amz-content-sha256";
    public const string DecodedContentLength = "x-amz-decoded-content-length";
    public const string ChecksumCrc32        = "x-amz-checksum-crc32";
    public const string TrailerSignature     = "x-amz-trailer-signature";
    public const string Date                 = "x-amz-date";
    public const string SecurityToken        = "x-amz-security-token";
}