namespace Amazon.Bedrock.Models;

using System.Text.Json.Serialization;

public sealed class PromptRouterTrace
{
    [JsonPropertyName("invokedModelId")]
    public string InvokedModelId { get; init; }
}