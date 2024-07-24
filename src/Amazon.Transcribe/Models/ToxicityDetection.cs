using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class ToxicityDetection
{
    // ALL
    [JsonPropertyName("ToxicityCategories")]
    public required List<string> ToxicityCategories { get; init; }
}
