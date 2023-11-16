namespace Amazon.S3;

public readonly struct ETag(string value)
{
    public string Value { get; } = value;

    public readonly byte[]? AsMD5()
    {
        if (Value is null || Value.Contains('-'))
        {
            return null;
        }

        // Generally the ETAG is the MD5 of the object -- hexadecimal encoded and wrapped in quotes.
        // If the object was uploaded using multipart upload then this is the MD5 all of the upload-part-md5s.

        // Multipart uploads also contain a dash

        // 1f8ada2ce841b291cfcd6b9b4b645044-2

        return Convert.FromHexString(Value.AsSpan().Trim('"'));
    }

    public static implicit operator string(ETag tag) => tag.Value;
}
