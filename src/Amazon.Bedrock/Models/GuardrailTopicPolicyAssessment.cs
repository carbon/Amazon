using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class GuardrailTopicPolicyAssessment
{
    [JsonPropertyName("topics")]
    public required List<GuardrailTopic> Topics { get; init; }
}