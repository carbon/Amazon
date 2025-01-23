namespace Amazon.Bedrock.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class GuardrailTraceAssessment
{
    [JsonPropertyName("inputAssessment")]
    public Dictionary<string, GuardrailAssessment> InputAssessment { get; set; }

    [JsonPropertyName("modelOutput")]
    public List<string> ModelOutput { get; set; }

    [JsonPropertyName("outputAssessments")]
    public Dictionary<string, List<GuardrailAssessment>> OutputAssessments { get; set; }
}
