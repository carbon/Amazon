using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class ContentBlock
{
    [JsonPropertyName("cachePoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CachePointBlock? CachePoint { get; init; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    [JsonPropertyName("document")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DocumentBlock? Document { get; init; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImageBlock? Image { get; init; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoBlock? Video { get; init; }

    [JsonPropertyName("reasoningContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReasoningContentBlock? ReasoningContent { get; init; }

    public static implicit operator ContentBlock(string text)
    {
        return new ContentBlock { Text = text };
    }

    public static implicit operator ContentBlock(DocumentBlock document)
    {
        return new ContentBlock { Document = document };
    }

    public static implicit operator ContentBlock(ImageBlock image)
    {
        return new ContentBlock { Image = image };
    }
  
    public static implicit operator ContentBlock(VideoBlock video)
    {
        return new ContentBlock { Video = video };
    }
}
