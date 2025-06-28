using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailUsage
{
    [JsonPropertyName("contentPolicyUnits")]
    public int ContentPolicyUnits { get; init; }

    [JsonPropertyName("contextualGroundingPolicyUnits")]
    public int ContextualGroundingPolicyUnits { get; init; }

    [JsonPropertyName("sensitiveInformationPolicyFreeUnits")]
    public int SensitiveInformationPolicyFreeUnits { get; init; }

    [JsonPropertyName("sensitiveInformationPolicyUnits")]
    public int SensitiveInformationPolicyUnits { get; init; }

    [JsonPropertyName("topicPolicyUnits")]
    public int TopicPolicyUnits { get; init; }

    [JsonPropertyName("wordPolicyUnits")]
    public int WordPolicyUnits { get; init; }
}