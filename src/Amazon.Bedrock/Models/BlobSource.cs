using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

// AKA ImageSource

public sealed class BlobSource
{
    [SetsRequiredMembers]
    public BlobSource(byte[] bytes)
    {
        Bytes = bytes;
    }

    [JsonPropertyName("bytes")]
    public required byte[] Bytes { get; init; }

    // s3Location { uri, bucketOwner }
    // e.g. s3://my-bucket/object-key
}