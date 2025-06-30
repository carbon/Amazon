using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public sealed class ListFoundationalModelsResult
{
    [JsonPropertyName("modelSummaries")]
    public required FoundationModelSummary[] ModelSummaries { get; init; }
}