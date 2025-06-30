using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public sealed class ListFoundationalModelsRequest
{
    [JsonPropertyName("byCustomizationType")]
    public string? ByCustomizationType { get; init; }

    [JsonPropertyName("byInferenceType")]
    public string? ByInferenceType { get; init; }

    [JsonPropertyName("byOutputModality")]
    public string? ByOutputModality { get; init; }

    [JsonPropertyName("byProvider")]
    public string? ByProvider { get; init; }

}