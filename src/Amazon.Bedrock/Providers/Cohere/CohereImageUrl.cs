using System.Text.Json.Serialization;

namespace Cohere;

public sealed class CohereImageUrl
{
    [JsonConstructor]
    public CohereImageUrl(string url) 
    {
        Url = url;
    }

    public CohereImageUrl(string mimeType, scoped ReadOnlySpan<byte> data)
    {
        Url = CohereDataUrl.Create(mimeType, data);
    }

    [JsonPropertyName("url")]
    public string Url { get; init; }

    public static implicit operator CohereImageUrl(string url)
    {
        return new CohereImageUrl(url);
    }
}