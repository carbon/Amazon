using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class DocumentBlock
{
    // pdf | csv | doc | docx | xls | xlsx | html | txt | md
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }

    [JsonPropertyName("citations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CitationsConfig? Citations { get; init; }
}