using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public sealed class ListFoundationalModelsRequest
{
    [JsonPropertyName("byCustomizationType")]
    public string? ByCustomizationType { get; set; }

    [JsonPropertyName("byInferenceType")]
    public string? ByInferenceType { get; set; }

    [JsonPropertyName("byOutputModality")]
    public string? ByOutputModality { get; set; }

    [JsonPropertyName("byProvider")]
    public string? ByProvider { get; set; }

}