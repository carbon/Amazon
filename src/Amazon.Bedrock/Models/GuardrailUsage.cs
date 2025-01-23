using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailUsage
{
    [JsonPropertyName("contentPolicyUnits")]
    public int ContentPolicyUnits { get; set; }

    [JsonPropertyName("contextualGroundingPolicyUnits")]
    public int ContextualGroundingPolicyUnits { get; set; }

    [JsonPropertyName("sensitiveInformationPolicyFreeUnits")]
    public int SensitiveInformationPolicyFreeUnits { get; set; }

    [JsonPropertyName("sensitiveInformationPolicyUnits")]
    public int SensitiveInformationPolicyUnits { get; set; }

    [JsonPropertyName("topicPolicyUnits")]
    public int TopicPolicyUnits { get; set; }

    [JsonPropertyName("wordPolicyUnits")]
    public int WordPolicyUnits { get; set; }
}