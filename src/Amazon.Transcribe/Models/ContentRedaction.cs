using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class ContentRedaction
{
    [JsonPropertyName("PiiEntityTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<PiiEntityType>? PiiEntityTypes { get; init; }

    // redacted | redacted_and_unredacted
    [JsonPropertyName("RedactionOutput")]
    public required string RedactionOutput { get; init; }

    [JsonPropertyName("RedactionType")]
    public required RedactionType RedactionType { get; init; }
}