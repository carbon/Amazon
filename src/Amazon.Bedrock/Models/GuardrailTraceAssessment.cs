namespace Amazon.Bedrock.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public sealed class GuardrailTraceAssessment
{
    [JsonPropertyName("inputAssessment")]
    public Dictionary<string, GuardrailAssessment> InputAssessment { get; init; }

    [JsonPropertyName("modelOutput")]
    public List<string> ModelOutput { get; init; }

    [JsonPropertyName("outputAssessments")]
    public Dictionary<string, List<GuardrailAssessment>> OutputAssessments { get; init; }
}
