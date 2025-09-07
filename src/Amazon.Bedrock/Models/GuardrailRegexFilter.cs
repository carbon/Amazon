#nullable disable

namespace Amazon.Bedrock.Models;

using System.Text.Json.Serialization;

public class GuardrailRegexFilter
{
    // BLOCKED | ANONYMIZED
    [JsonPropertyName("action")]
    public string Action { get; init; }

    [JsonPropertyName("match")]
    public string Match { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("regex")]
    public string Regex { get; init; }
}