using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class BlobSource
{
    public BlobSource(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        Bytes = bytes;
    }

    public BlobSource(S3Location s3Location)
    {
        ArgumentNullException.ThrowIfNull(s3Location);

        S3Location = s3Location;
    }

    [JsonPropertyName("bytes")]
    public byte[]? Bytes { get; init; }

    [JsonPropertyName("s3Location")]
    public S3Location? S3Location { get; init; }

    // s3Location { uri, bucketOwner }
    // e.g. s3://my-bucket/object-key

    public static implicit operator BlobSource(byte[] bytes)
    {
        return new BlobSource(bytes);
    }

    public static implicit operator BlobSource(S3Location s3Location)
    {
        return new BlobSource(s3Location);
    }
}

// Used for ImageSource | VideoSource