using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Nova;

public sealed class NovaContent
{
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    [JsonPropertyName("document")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DocumentObject? Document { get; init; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaObject? Image { get; init; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MediaObject? Video { get; init; }

    public static implicit operator NovaContent(string text)
    {
        return new NovaContent { Text = text };
    }
}

public class MediaObject
{
    public MediaObject() { }

    [SetsRequiredMembers]
    public MediaObject(string format, BlobSource source)
    {
        Format = format;
        Source = source;
    }

    // "jpeg" | "png" | "gif" | "webp"
    // "mkv" | "mov" | "mp4" | "webm" | "three_gp" | "flv" | "mpeg" | "mpg" | "wmv"
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }

}

public class DocumentObject
{
   
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    // docs only
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }
}


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