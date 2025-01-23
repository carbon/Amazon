using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailAssessment
{
    [JsonPropertyName("contentPolicy")]
    public GuardrailContentPolicyAssessment? ContentPolicy { get; set; }

    [JsonPropertyName("contextualGroundingPolicy")]
    public GuardrailContextualGroundingPolicyAssessment? ContextualGroundingPolicy { get; set; }

    [JsonPropertyName("invocationMetrics")]
    public GuardrailInvocationMetrics? InvocationMetrics { get; set; }

    [JsonPropertyName("sensitiveInformationPolicy")]
    public GuardrailSensitiveInformationPolicyAssessment? SensitiveInformationPolicy { get; set; }

    [JsonPropertyName("topicPolicy")]
    public GuardrailTopicPolicyAssessment? TopicPolicy { get; set; }

    [JsonPropertyName("wordPolicy")]
    public GuardrailWordPolicyAssessment? WordPolicy { get; set; }
}
