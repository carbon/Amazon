using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailAssessment
{
    [JsonPropertyName("contentPolicy")]
    public GuardrailContentPolicyAssessment? ContentPolicy { get; init; }

    [JsonPropertyName("contextualGroundingPolicy")]
    public GuardrailContextualGroundingPolicyAssessment? ContextualGroundingPolicy { get; init; }

    [JsonPropertyName("invocationMetrics")]
    public GuardrailInvocationMetrics? InvocationMetrics { get; init; }

    [JsonPropertyName("sensitiveInformationPolicy")]
    public GuardrailSensitiveInformationPolicyAssessment? SensitiveInformationPolicy { get; init; }

    [JsonPropertyName("topicPolicy")]
    public GuardrailTopicPolicyAssessment? TopicPolicy { get; init; }

    [JsonPropertyName("wordPolicy")]
    public GuardrailWordPolicyAssessment? WordPolicy { get; init; }
}
