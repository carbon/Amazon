using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public sealed class FoundationModelSummary
{
    [JsonPropertyName("modelArn")]
    public required string ModelArn { get; init; }

    [JsonPropertyName("modelId")]
    public required string ModelId { get; init; }

    [JsonPropertyName("inputModalities")]
    public Modality[]? InputModalities { get; init; }

    [JsonPropertyName("outputModalities")]
    public Modality[]? OutputModalities { get; init; }

    [JsonPropertyName("modelLifecycle")]
    public FoundationModelLifecycle? ModelLifecycle { get; init; }

    [JsonPropertyName("providerName")]
    public string? ProviderName { get; init; }

    [JsonPropertyName("modelName")]
    public string? ModelName { get; init; }

    [JsonPropertyName("inferenceTypesSupported")]
    public InferenceType[]? InferenceTypesSupported { get; init; }

    [JsonPropertyName("responseStreamingSupported")]
    public bool ResponseStreamingSupported { get; init; }
}