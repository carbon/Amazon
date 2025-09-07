#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class PromptRouterTrace
{
    [JsonPropertyName("invokedModelId")]
    public string InvokedModelId { get; init; }
}