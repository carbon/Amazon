using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class Image
{
    public Image(byte[] blob)
    {
        if (blob.Length > 5_242_880)
        {
            throw new ArgumentOutOfRangeException(nameof(blob), blob.Length, "Must be less than 5,242,880 bytes");
        }

        Bytes = blob;
    }

    public Image(S3Object s3Object)
    {
        S3Object = s3Object;
    }

    [JsonPropertyName("Bytes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? Bytes { get; }

    [JsonPropertyName("S3Object")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public S3Object? S3Object { get; }
}