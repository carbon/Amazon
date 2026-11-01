using System.Text.Json.Serialization;

namespace Cohere;

public sealed class CoherePart
{
    [JsonPropertyName("type")]
    public CoherePartType Type { get; init; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    [JsonPropertyName("image_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CohereImageUrl? ImageUrl { get; init; }

    public static CoherePart Image(string mimeType, scoped ReadOnlySpan<byte> data)
    {
        return new CoherePart { 
            Type = CoherePartType.ImageUrl, 
            ImageUrl = new CohereImageUrl(mimeType, data) 
        };
    }

    public static implicit operator CoherePart(string text)
    {
        return new CoherePart { Type = CoherePartType.Text, Text = text };
    }
}