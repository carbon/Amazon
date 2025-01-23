using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Bedrock;

public sealed class ConverseRequest
{
    [JsonPropertyName("additionalModelRequestFields")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? AdditionalModelRequestFields { get; set; }

    [JsonPropertyName("additionalModelResponseFieldPaths")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? AdditionalModelResponseFieldPaths { get; set; }

    [JsonPropertyName("guardrailConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GuardrailConfiguration? GuardrailConfig { get; set; }

    [JsonPropertyName("inferenceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InferenceConfiguration? InferenceConfig { get; set; }

    [JsonPropertyName("messages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Message>? Messages { get; set; }

    [JsonPropertyName("performanceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PerformanceConfiguration? PerformanceConfig { get; set; }

    /// <summary>
    /// Contains a map of variables in a prompt from Prompt management to objects containing the values to fill in for them when running model invocation. 
    /// This field is ignored if you don't specify a prompt resource in the modelId field.
    /// </summary>
    [JsonPropertyName("promptVariables")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, JsonElement>? PromptVariables { get; set; }

    /// <summary>
    /// Key-value pairs that you can use to filter invocation logs.
    /// </summary>
    [JsonPropertyName("requestMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? RequestMetadata { get; set; }

    [JsonPropertyName("system")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<SystemContentBlock>? System { get; set; }

    [JsonPropertyName("toolConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ToolConfiguration? ToolConfig { get; set; }
}
