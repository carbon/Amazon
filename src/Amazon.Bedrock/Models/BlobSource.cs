using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

// AKA ImageSource

public sealed class BlobSource
{
    [SetsRequiredMembers]
    public BlobSource(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        Bytes = bytes;
    }

    [JsonPropertyName("bytes")]
    public required byte[] Bytes { get; init; }

    // s3Location { uri, bucketOwner }
    // e.g. s3://my-bucket/object-key

    public static implicit operator BlobSource(byte[] bytes)
    {
        return new BlobSource(bytes);
    }
}