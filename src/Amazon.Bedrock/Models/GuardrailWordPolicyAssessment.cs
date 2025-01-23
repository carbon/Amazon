using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class GuardrailWordPolicyAssessment
{
    [JsonPropertyName("customWords")]
    public required List<GuardrailCustomWord> CustomWords { get; init; }

    [JsonPropertyName("managedWordLists")]
    public required List<GuardrailManagedWord> ManagedWordLists { get; init; }
}